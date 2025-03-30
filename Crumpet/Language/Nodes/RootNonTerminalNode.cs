using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class RootNonTerminalNode : NonTerminalNode, INonTerminalNodeFactory
{
    public DeclarationNode[] Declarations { get; }
    
    public RootNonTerminalNode(IEnumerable<DeclarationNode> declarationNodes)
    {
        Declarations = declarationNodes.ToArray();
        ImplicitChildren.AddRange(Declarations);
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<RootNonTerminalNode>(
            new ZeroOrMoreConstraint(new NonTerminalConstraint<DeclarationNode>()), 
            GetNodeConstructor<RootNonTerminalNode>());
    }
}