using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.InstanceValues;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language;

namespace Crumpet.Interpreter.Functions;

public class Function
{
    private readonly IInstruction[] m_instructions;
    public FunctionDefinition Definition { get; }

    public Function(FunctionDefinition definition, IEnumerable<IInstruction> instructions)
    {
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
    public ExecutableUnit CreateInvokableUnit(ExecutionContext context, InstanceReference[] arguments)
    {
        if (arguments.Length != Definition.Parameters.Count)
            // default on source location will occur if it's the first invocable called
            throw new InterpreterException(context.CurrentUnit?.SourceLocation ?? new SourceLocation(), ExceptionConstants.INVALID_ARGUMENT_COUNT.Format(Definition.Parameters.Count, arguments.Length));

        ExecutableUnit unit = new ExecutableUnit(context, m_instructions, Definition.SourceLocation);
        
        for (int i = 0; i < Definition.Parameters.Count; i++)
        {
            TypeInfo argType = arguments[i].Type;
            TypeInfo defType = Definition.Parameters[i].Type;
            VariableModifier modifier = Definition.Parameters[i].VariableModifier;

            // target reference
            InstanceReference convertedArg;
            
            if (argType == defType)
            {
                // set as Variable will do copy or reference assign later
                convertedArg = arguments[i];
            }
            else if (argType.ConvertableTo(Definition.Parameters[i].Type))
            {
                // cannot use a convert with a reference or pointer
                if (modifier != VariableModifier.COPY)
                    throw new InterpreterException(context.CurrentUnit?.SourceLocation ?? new SourceLocation(), ExceptionConstants.CONVERT_REFERENCE_ASSIGN);

                // convert instance with a copy of 
                convertedArg = defType.ConvertInstance(arguments[i]);
            }
            else
            {
                throw new InterpreterException(
                    context.CurrentUnit?.SourceLocation ?? new SourceLocation(), 
                    ExceptionConstants.INVALID_ARGUMENT_TYPE.Format(i, defType, argType));
            }
            
            // create the new instance in the function's scope
            // then assign the value 
            unit.Scope.Create(new VariableInfo(Definition.Parameters[i]));
            unit.Scope.GetVariable(Definition.Parameters[i].Name).Instance = convertedArg;
        }

        return unit;
    }
}