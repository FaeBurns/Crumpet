using Crumpet.Interpreter.Variables.Types;

namespace Crumpet.Interpreter.Functions;

public abstract class FunctionTemplate
{
    public abstract string Name { get; }
    public IReadOnlyList<ParameterTemplate> Parameters { get; }
    public IReadOnlyList<string> TypeParameters { get; }

    protected FunctionTemplate(IReadOnlyList<ParameterTemplate> parameters, IReadOnlyList<string> typeParameters)
    {
        Parameters = parameters;
        TypeParameters = typeParameters;
    }

    public abstract Function Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> typeArgs);
}