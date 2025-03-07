using System.Diagnostics;
using Crumpet.Language.Nodes;
using Parser;

namespace Crumpet.Interpreter.Preparse;

public class TypeBuilder
{
    private readonly IEnumerable<TypeDeclarationNode> m_typeDeclarations;

    public TypeBuilder(IEnumerable<ASTNode> nodes)
    {
        m_typeDeclarations = nodes.OfType<TypeDeclarationNode>();
    }

    public TypeResolver GetTypeDefinitions()
    {
        PlaceholderTypeResolver resolver = new PlaceholderTypeResolver();

        foreach (TypeDeclarationNode node in m_typeDeclarations)
        {
            resolver.RegisterType(node);
        }

        // should never return false
        bool clean = resolver.ReplacePlaceholders();
        Debug.Assert(clean);

        resolver.UpdateFieldsInUserTypes();

        return resolver.Build();
    }
}