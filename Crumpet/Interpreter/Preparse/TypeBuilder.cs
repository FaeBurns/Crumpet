using System.Diagnostics;
using Crumpet.Exceptions;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Interpreter.Variables.Types.Templates;
using Crumpet.Language.Nodes;
using Parser;
using Shared;

namespace Crumpet.Interpreter.Preparse;

public class TypeBuilder
{
    private readonly IEnumerable<TypeDeclarationNode> m_typeDeclarations;
    private readonly Dictionary<string, TypeTemplate> m_templates = new Dictionary<string, TypeTemplate>();

    public TypeBuilder(IEnumerable<ASTNode> nodes)
    {
        m_typeDeclarations = nodes.OfType<TypeDeclarationNode>();
        
        m_templates.Add("string", new DirectTypeTemplate(BuiltinTypeInfo.String));
        m_templates.Add("int", new DirectTypeTemplate(BuiltinTypeInfo.Int));
        m_templates.Add("float", new DirectTypeTemplate(BuiltinTypeInfo.Float));
        m_templates.Add("bool", new DirectTypeTemplate(BuiltinTypeInfo.Bool));
        m_templates.Add("void", new DirectTypeTemplate(new VoidTypeInfo()));
        
        m_templates.Add("map", new DictionaryTypeTemplate());
    }

    public TypeResolver GetTypeDefinitions()
    {
        foreach (TypeDeclarationNode node in m_typeDeclarations)
        {
            m_templates.Add(node.Name.Terminal, BuildEmptyUserType(node));
        }
        
        foreach (TypeDeclarationNode node in m_typeDeclarations)
        {
            CompleteUserType(node, (UserObjectTypeTemplate)GetTemplate(node.Name.Terminal));
        }

        return new TypeResolver(m_templates);
    }

    private UserObjectTypeTemplate BuildEmptyUserType(TypeDeclarationNode node)
    {
        return new UserObjectTypeTemplate(
            node.Name.Terminal,
            node.GenericTypesDeclaration.GenericTypeNames.Select(t => t.Terminal).ToArray(),
            []
            );
    }

    private void CompleteUserType(TypeDeclarationNode node, UserObjectTypeTemplate template)
    {
        template.Fields = node.Fields.Select(f => BuildFieldTemplate(node, f)).ToArray();
    }

    private FieldTemplate BuildFieldTemplate(TypeDeclarationNode typeNode, TypeDeclarationFieldNode fieldNode)
    {
        return new FieldTemplate(fieldNode.Name.Terminal, TypeNodeToTemplate(fieldNode.Type), fieldNode.VariableModifier);
    }

    private TypeTemplate TypeNodeToTemplate(TypeNode node)
    {
        if (node.TypeArgs.TypeArguments.Length == 0)
        {
            // if no template was found it must be a generic template
            // e.g. T will not be found by TryGetTemplate so it must be a replaceable generic
            TypeTemplate? template = TryGetTemplate(node.FullName);
            if (template is null)
                return new GenericReplaceableTypeTemplate(node.FullName);
        }

        TypeTemplate[] args = new TypeTemplate[node.TypeArgs.TypeArguments.Length];
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = TypeNodeToTemplate(node.TypeArgs.TypeArguments[i]);
        }
        
        return new TypeWithTypeArgsTemplate(GetTemplate(node.FullName), args);
    }

    private TypeTemplate GetTemplate(string name)
    {
        return m_templates.GetValueOrDefault(name) ?? throw new TypeNotFoundException(name, ExceptionConstants.UNKOWN_TYPE.Format(name));
    }

    private TypeTemplate? TryGetTemplate(string name)
    {
        return m_templates.GetValueOrDefault(name);
    }
}