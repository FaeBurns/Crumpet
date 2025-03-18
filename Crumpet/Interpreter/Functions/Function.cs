using Crumpet.Exceptions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;
using Shared;

namespace Crumpet.Interpreter.Functions;

public class Function
{
    private readonly Instruction[] m_instructions;
    public FunctionDefinition Definition { get; }

    public Function(FunctionDefinition definition, IEnumerable<Instruction> instructions)
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
            throw new InterpreterException(context.CurrentUnit?.UnitLocation ?? new SourceLocation(), ExceptionConstants.INVALID_ARGUMENT_COUNT.Format(Definition.Parameters.Count, arguments.Length));

        ExecutableUnit unit = new ExecutableUnit(context, m_instructions, Definition.SourceLocation);
        unit.AcceptsReturn = true;

        for (int i = 0; i < Definition.Parameters.Count; i++)
        {
            TypeInfo argType = arguments[i].Type;
            TypeInfo defType = Definition.Parameters[i].Type;
            VariableModifier modifier = Definition.Parameters[i].VariableModifier;

            // target reference
            Variable convertedArg = Variable.CreateModifier(defType, modifier, arguments[i]);

            if (argType == defType)
            {
                // set value
                convertedArg.Value = arguments[i];
            }
            else if (argType.ConvertableTo(Definition.Parameters[i].Type))
            {
                // cannot use a convert with a reference or pointer
                if (modifier != VariableModifier.COPY)
                    throw new InterpreterException(context.CurrentUnit?.UnitLocation ?? new SourceLocation(), ExceptionConstants.CONVERT_REFERENCE_ASSIGN);

                // convert instance with a copy of
                convertedArg.Value = defType.ConvertValidObject(argType, arguments[i].Value);
            }
            else
            {
                throw new InterpreterException(context.CurrentUnit?.UnitLocation,
                    ExceptionConstants.INVALID_ARGUMENT_TYPE.Format(i, defType, argType));
            }

            // create the new instance in the function's scope
            // then assign the value
            unit.Scope.Add(Definition.Parameters[i].Name, convertedArg);
        }

        return unit;
    }
}