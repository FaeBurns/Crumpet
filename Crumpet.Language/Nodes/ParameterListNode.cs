using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

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
        yield return new NonTerminalDefinition("parameterList",
            new SequenceConstraint(
                new NonTerminalConstraint("parameter"),
                new ZeroOrMoreConstraint(
                    new SequenceConstraint(
                        new RawTerminalConstraint(","),
                        new NonTerminalConstraint("parameter")))),
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