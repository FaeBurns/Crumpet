using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;

namespace Crumpet.Language.Nodes;

public class ParameterListNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ParameterNode[] Parameters { get; }
    
    public ParameterListNode(ParameterNode first, IEnumerable<ParameterNode> others)
    {
        Parameters = others.Prepend(first).ToArray();
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ParameterListNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<ParameterNode>(),
                new ZeroOrMoreConstraint(
                    new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.COMMA),
                        new NonTerminalConstraint<ParameterNode>()))),
            GetNodeConstructor<ParameterListNode>());
    }

    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        foreach (ParameterNode node in Parameters)
        {
            yield return node;
        }
    }
}