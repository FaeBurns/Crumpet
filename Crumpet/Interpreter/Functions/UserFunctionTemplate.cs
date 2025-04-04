using Crumpet.Exceptions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
using Shared;

namespace Crumpet.Interpreter.Functions;

public class UserFunctionTemplate : FunctionTemplate
{
    private readonly Instruction[] m_instructions;
    public FunctionDefinition Definition { get; }

    public override string Name => Definition.Name;

    public UserFunctionTemplate(FunctionDefinition definition, IEnumerable<Instruction> instructions) : base(definition.Parameters, definition.TypeParameterNames)
    {
        Definition = definition;
        m_instructions = instructions.ToArray();
    }
    
    public override Function Construct(TypeResolver resolver, IReadOnlyList<TypeInfo> typeArgs)
    {
        if (Definition.TypeParameterNames.Count != typeArgs.Count)
            throw new GenericsException(ExceptionConstants.GENERIC_ARGUMENT_COUNT_MISMATCH.Format(Definition.TypeParameterNames.Count, typeArgs.Count));
        
        resolver.PushGenericArguments(new GenericTypeContext(TypeParameters, typeArgs));
        
        ParameterDefinition[] parameters = new ParameterDefinition[Definition.Parameters.Count];
        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterTemplate template = Definition.Parameters[i];
            parameters[i] = new ParameterDefinition(template.Name, resolver.TemplateConstructOrCache(template.Template, typeArgs), template.Modifier);
        }
        
        Dictionary<string, TypeInfo> typeParams = new Dictionary<string, TypeInfo>();
        for (int i = 0; i < Definition.TypeParameterNames.Count; i++)
        {
            typeParams.Add(Definition.TypeParameterNames[i], typeArgs[i]);
        }
        
        UserFunction function = new UserFunction(Name, m_instructions, resolver.TemplateConstructOrCache(Definition.ReturnType, []), parameters, typeParams, Definition.SourceLocation);
        
        // need to pop before returning
        resolver.PopGenericArguments();
        return function;
    }
}