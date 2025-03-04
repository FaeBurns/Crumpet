using System.Diagnostics;
using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Interpreter.Variables;
using Crumpet.Interpreter.Variables.Types;
using Crumpet.Language.Nodes;

namespace Crumpet.Interpreter.Preparse;

public class TypeBuilder
{
    private readonly IEnumerable<TypeDeclarationNode> m_typeDeclarations;
    
    public TypeBuilder(NonTerminalNode rootNode)
    {
        IEnumerable<ASTNode> nodeSequence = new NodeSequenceEnumerator(rootNode);
        m_typeDeclarations = nodeSequence.OfType<TypeDeclarationNode>();
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