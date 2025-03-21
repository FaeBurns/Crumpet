using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Parser.Elements;
using Parser.Nodes;
using Shared;
using Shared.Exceptions;

namespace Parser;

public class NonTerminalInstanceConstructor<T> where T : Enum
{
    private readonly NonTerminalDefinition m_definition;

    public NonTerminalInstanceConstructor(NonTerminalDefinition definition)
    {
        m_definition = definition;
    }

    public ASTNode? Construct(ObjectStream<TerminalNode<T>> stream, ASTNodeRegistry<T> registry)
    {
        if (stream.IsAtEnd)
            return null;
        
        using PositionSaver<TerminalNode<T>> position = stream.ConstrainPosition();
        TerminalNode<T> initialToken = stream.PeekCurrent();
     
        // do check on declaring type as that's gonna be the one the constructor is in
        // as long as they're not the same
        // handles checking for variants
        ParserDebuggerHelper<T>.BreakIfNecessaryTrying(m_definition.Type);
        if (m_definition.Constructor.DeclaringType! != m_definition.Type)
            ParserDebuggerHelper<T>.BreakIfNecessaryTrying(m_definition.Constructor.DeclaringType!);
        
        // inspect the rule's constraint
        // if the constraint does not pass then check the next one
        ParserElement? element = m_definition.Constraint.WalkStream(stream, registry);
        if (element == null)
            return null;
        
        // another debug step
        ParserDebuggerHelper<T>.BreakIfNecessarySuccess(m_definition.Type);
        if (m_definition.Constructor.DeclaringType! != m_definition.Type)
            ParserDebuggerHelper<T>.BreakIfNecessarySuccess(m_definition.Constructor.DeclaringType!);

        // transformForConstrutor handles inner info
        object[] arguments = element.TransformForConstructor().ToArray();

        ParameterInfo[] parameters = m_definition.Constructor.GetParameters();
        
        // throw if there are too many arguments
        if (arguments.Length > parameters.Length)
            throw new ArgumentException(ExceptionConstants.INVALID_ARGUMENT_COUNT.Format(parameters.Length, arguments.Length));
            
        // convert any arrays to the expected type
        object[] parameterizedArguments = new object[parameters.Length];
        
        for (int i = 0; i < arguments.Length; i++)
        {
            parameterizedArguments[i] = ConvertParameter(arguments[i], parameters[i]);
        }

        // construct node
        // if the construction fails then return nothing and reset position (automatic)
        NonTerminalNode node;
        try
        {
            node = (NonTerminalNode)m_definition.Constructor.Invoke(parameterizedArguments);
            
            // take start location from initial token
            // and last from the stream
            // ensure that the first element is taken if it would go negative
            TerminalNode<T> lastTerminal = stream[Math.Max(stream.Position - 1, 0)];
            node.Location = SourceLocation.FromRange(initialToken.Location, lastTerminal.Location);
        }
        catch (Exception e)
        {
            // here the node should have been constructed so throw an exception - something went very wrong
            throw new ParserException(ExceptionConstants.NODE_CONSTRUCTOR_FAILED.Format(m_definition.Type), initialToken.Location, e);
        }
            
        // if the node was successfully constructed then don't reset the position and return the new node
        position.ConsumePosition();
        return node;
    }
    
    private object ConvertParameter(object argument, ParameterInfo parameter)
    {
        Type parameterType = parameter.ParameterType;
        if (argument is IEnumerable<object> array && parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            // get the first generic argument
            Type genericType = parameterType.GetGenericArguments()[0];
            object[] objArray = array as object[] ?? array.ToArray();

            if (genericType.IsGenericType)
            {
                Type[] innerGenericTypes = genericType.GetGenericArguments();
                if (innerGenericTypes.Length >= 2)
                {
                    object[] tupleArray = ConvertToTupleArray(objArray, innerGenericTypes.Length);
                    return ConvertToSingleTypeArray(tupleArray, genericType);
                }
            }
            else
            {
                return ConvertToSingleTypeArray(objArray, genericType);
            }

            throw new UnreachableException();
        }
        else
        {
            return argument;
        }
    }

    private object ConvertToSingleTypeArray(object[] elements, Type arrayType)
    {
        // populate a new array with the arguments
        Array typedArray = Array.CreateInstance(arrayType, elements.Length);
        Array.Copy(elements, typedArray, elements.Length);

        return typedArray;
    }

    private object[] ConvertToTupleArray(object[] elements, int elementsPerTuple)
    {
        // this shit don't work
        // need to case to Tuple<known type, known type...> but this only returns Tuple<object, object...>
        // cannot implicitly cast between
        List<object> tuples = new List<object>(elements.Length / elementsPerTuple);
        for (int i = 0; i < elements.Length; i += elementsPerTuple)
        {
            object tuple = elementsPerTuple switch
            {
                2 => (elements[i], elements[i + 1]),
                3 => (elements[i], elements[i + 1], elements[i + 2]),
                4 => (elements[i], elements[i + 1], elements[i + 2], elements[i + 3]),
                5 => (elements[i], elements[i + 1], elements[i + 2], elements[i + 3], elements[i + 4]),
                6 => (elements[i], elements[i + 1], elements[i + 2], elements[i + 3], elements[i + 4], elements[i + 5]),
                7 => (elements[i], elements[i + 1], elements[i + 2], elements[i + 3], elements[i + 4], elements[i + 5], elements[i + 6]),
                _ => throw new ArgumentException()
            };
            tuples.Add(tuple);
        }

        return tuples.ToArray();
    }
}