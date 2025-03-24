using Crumpet.Exceptions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Functions;

public class UserFunction : Function
{
    private readonly Instruction[] m_instructions;
    public FunctionDefinition Definition { get; }
    
    public override string Name => Definition.Name;

    public UserFunction(FunctionDefinition definition, IEnumerable<Instruction> instructions) : base(definition.Parameters.Select(p => new ParameterInfo(p.Type, p.VariableModifier)).ToArray())
    {
        // get instructions from child nodes
        m_instructions = instructions.ToArray();
        Definition = definition;
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
        if (arguments.Length != Definition.Parameters.Count)
            // default on source location will occur if it's the first invocable called
            throw new ArgumentException(ExceptionConstants.INVALID_ARGUMENT_COUNT.Format(Definition.Parameters.Count, arguments.Length));

        ExecutableUnit unit = new ExecutableUnit(context, m_instructions, Definition.SourceLocation, true);

        for (int i = 0; i < Definition.Parameters.Count; i++)
        {
            // target reference
            Variable convertedArg = Variable.CreateCopy(arguments[i]);

            // create the new instance in the function's scope
            // then assign the value
            unit.Scope.Add(Definition.Parameters[i].Name, convertedArg);
        }

        return unit;
    }
}