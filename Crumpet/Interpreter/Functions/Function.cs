using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Functions;

public abstract class Function
{
    public abstract string Name { get; }

    public IReadOnlyList<ParameterInfo> Parameters { get; }

    protected Function(IEnumerable<ParameterInfo> parameters)
    {
        Parameters = parameters.ToArray();
    }
}