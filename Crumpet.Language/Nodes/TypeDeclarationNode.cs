using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;
using Crumpet.Parser;
using Crumpet.Parser.NodeConstraints;
using Crumpet.Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class TypeDeclarationNode : NonTerminalNode, INonTerminalNodeFactory
{
    public IdentifierNode Name { get; }
    public TypeDeclarationFieldNode[] Fields { get; }

    public TypeDeclarationNode(IdentifierNode name, IEnumerable<TypeDeclarationFieldNode> fields)
    {
        Name = name;
        Fields = fields.ToArray();
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<TypeDeclarationNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_STRUCT),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.LBRACK),
                new ZeroOrMoreConstraint(new NonTerminalConstraint<TypeDeclarationFieldNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.RBRACK)),
            GetNodeConstructor<TypeDeclarationNode>());
    }

    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        yield return Name;
        foreach (TypeDeclarationFieldNode node in Fields)
        {
            yield return node;
        }
    }
}