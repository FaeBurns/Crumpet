using Crumpet.Interpreter.Variables.Types;
using Crumpet.Interpreter.Variables.Types.Templates;

namespace Crumpet.Interpreter.Functions;

public class BuiltInFunction : Function
{
    private readonly Action<InterpreterExecutionContext, IReadOnlyList<TypeInfo>> m_function;

    public override string Name { get; }

    public BuiltInFunction(string name, Action<InterpreterExecutionContext, IReadOnlyList<TypeInfo>> function, params IEnumerable<ParameterDefinition> parameters) : base(parameters, 0)
    {
        Name = name;
        m_function = function;
    }
    
    public BuiltInFunction(string name, Action<InterpreterExecutionContext, IReadOnlyList<TypeInfo>> function, int typeArgCount, params IEnumerable<ParameterDefinition> parameters) : base(parameters, typeArgCount)
    {
        Name = name;
        m_function = function;
    }
    
    public void Invoke(InterpreterExecutionContext context, IReadOnlyList<TypeInfo> typeArgs)
    {
        m_function.Invoke(context, typeArgs);
    }
}