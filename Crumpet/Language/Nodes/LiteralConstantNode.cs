using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class LiteralConstantNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    private readonly ASTNode m_childRef;

    protected LiteralConstantNode(ASTNode childRef) : base(childRef)
    {
        m_childRef = childRef;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<LiteralConstantNode>(
            new OrConstraint(
                new CrumpetTerminalConstraint(CrumpetToken.STRING),
                new CrumpetTerminalConstraint(CrumpetToken.INT),
                new CrumpetTerminalConstraint(CrumpetToken.FLOAT),
                new CrumpetTerminalConstraint(CrumpetToken.BOOL),
                new CrumpetTerminalConstraint(CrumpetToken.NULL)
            ),
            GetNodeConstructor<LiteralConstantNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return m_childRef;
    }
}