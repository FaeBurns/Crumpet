using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class DeclarationNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ASTNode Variant { get; }

    protected DeclarationNode(ASTNode variant) : base(variant)
    {
        Variant = variant;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<DeclarationNode>(new NonTerminalConstraint<FunctionDeclarationNode>(), GetNodeConstructor<DeclarationNode>());
        yield return new NonTerminalDefinition<DeclarationNode>(new NonTerminalConstraint<TypeDeclarationNode>(), GetNodeConstructor<DeclarationNode>());
        yield return new NonTerminalDefinition<DeclarationNode>(new NonTerminalConstraint<IncludeDeclarationNode>(), GetNodeConstructor<DeclarationNode>());
    }
}