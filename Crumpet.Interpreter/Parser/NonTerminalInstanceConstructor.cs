using System.Diagnostics;
using System.Reflection;
using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Parser.Elements;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser;

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
        ParserDebuggerHelper<T>.BreakIfNecessary(m_definition.Type);
        if (m_definition.Constructor.DeclaringType! != m_definition.Type)
            ParserDebuggerHelper<T>.BreakIfNecessary(m_definition.Constructor.DeclaringType!);
        
        // inspect the rule's constraint
        // if the constraint does not pass then check the next one
        ParserElement? element = m_definition.Constraint.WalkStream(stream, registry);
        if (element == null)
            return null;

        // transformForConstrutor handles inner info
        object[] arguments = element.TransformForConstructor().ToArray();

        ParameterInfo[] parameters = m_definition.Constructor.GetParameters();

        if (arguments.Length != parameters.Length && Debugger.IsAttached)
        {
            Debugger.Break();
            // redo but now with debugger
            arguments = element.TransformForConstructor().ToArray();
        }
            
        // convert any arrays to the expected type
        object[] parameterizedArguments = new object[arguments.Length];
            
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
            TerminalNode<T> lastTerminal = stream[stream.Position - 1];
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
            
            // populate a new array with the arguments
            Array typedArray = Array.CreateInstance(genericType, objArray.Length);
            Array.Copy(objArray, typedArray, objArray.Length);

            return typedArray;
        }
        else
        {
            return argument;
        }
    }
}