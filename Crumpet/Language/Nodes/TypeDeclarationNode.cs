using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class TypeDeclarationNode : NonTerminalNode, INonTerminalNodeFactory
{
    public IdentifierNode Name { get; }
    public GenericTypeDeclarationListNode GenericTypesDeclaration { get; }
    public TypeDeclarationFieldNode[] Fields { get; }

    public TypeDeclarationNode(IdentifierNode name, GenericTypeDeclarationListNode genericTypesDeclaration, IEnumerable<TypeDeclarationFieldNode> fields)
    {
        Name = name;
        GenericTypesDeclaration = genericTypesDeclaration;
        Fields = fields.ToArray();
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<TypeDeclarationNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_STRUCT),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new NonTerminalConstraint<GenericTypeDeclarationListNode>(),
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

        yield return GenericTypesDeclaration;
    }
}