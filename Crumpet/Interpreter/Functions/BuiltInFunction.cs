namespace Crumpet.Interpreter.Functions;

public class BuiltInFunction : Function
{
    private readonly Action<InterpreterExecutionContext> m_function;

    public override string Name { get; }

    public BuiltInFunction(string name, Action<InterpreterExecutionContext> function, params IEnumerable<ParameterInfo> parameters) : base(parameters, 0)
    {
        Name = name;
        m_function = function;
    }
    
    public BuiltInFunction(string name, Action<InterpreterExecutionContext> function, int typeArgCount, params IEnumerable<ParameterInfo> parameters) : base(parameters, typeArgCount)
    {
        Name = name;
        m_function = function;
    }
    
    public void Invoke(InterpreterExecutionContext context)
    {
        m_function.Invoke(context);
    }
}