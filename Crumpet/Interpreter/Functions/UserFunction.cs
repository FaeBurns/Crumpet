using Crumpet.Exceptions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Functions;

public class UserFunction : Function
{
    public SourceLocation SourceLocation { get; }
    private readonly string m_name;
    private readonly Instruction[] m_instructions;

    public override string Name => m_name;
    
    public TypeInfo ReturnType { get; }
    public Dictionary<string, TypeInfo> TypeParameters { get; }

    public UserFunction(string name, IEnumerable<Instruction> instructions, TypeInfo returnType, IReadOnlyList<ParameterDefinition> parameters, ICollection<KeyValuePair<string, TypeInfo>> typeParams, SourceLocation sourceLocation) : base(parameters, typeParams.Count())
    {
        ReturnType = returnType;
        SourceLocation = sourceLocation;
        m_name = name;
        m_instructions = instructions.ToArray();
        TypeParameters = typeParams.ToDictionary();
    }
    
    /// <summary>
    /// Creates an <see cref="ExecutableUnit"/> for this function.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="arguments"></param>
    /// <returns></returns>
    /// <exception cref="InterpreterException">Argument count mismatch.</exception>
    public ExecutableUnit CreateInvokableUnit(InterpreterExecutionContext context, Variable[] arguments)
    {
        if (arguments.Length != Parameters.Count)
            // default on source location will occur if it's the first invocable called
            throw new ArgumentException(ExceptionConstants.INVALID_ARGUMENT_COUNT.Format(Parameters.Count, arguments.Length));

        ExecutableUnit unit = new ExecutableUnit(context, m_instructions, TypeParameters, SourceLocation, true);

        for (int i = 0; i < Parameters.Count; i++)
        {
            // target reference
            Variable convertedArg = Variable.CreateCopy(arguments[i]);

            // create the new instance in the function's scope
            // then assign the value
            unit.Scope.Add(Parameters[i].Name, convertedArg);
        }

        return unit;
    }
}