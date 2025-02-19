using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;

namespace Crumpet.Language.Nodes.Expressions;

public class SumExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{
    public MultExpressionNode Primary { get; }
    public TerminalNode<CrumpetToken>? Sugar { get; }
    public MultExpressionNode? Secondary { get; }
    
    public SumExpressionNode(MultExpressionNode primary, TerminalNode<CrumpetToken> sugar, MultExpressionNode secondary) : base(primary, sugar, secondary)
    {
        Primary = primary;
        Sugar = sugar;
        Secondary = secondary;
    }

    public SumExpressionNode(MultExpressionNode primary) : base(primary)
    {
        Primary = primary;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        // yield return new NonTerminalDefinition<SumExpressionNode>(
        //     new SequenceConstraint(
        //         new NonTerminalConstraint<MultExpressionNode>(),
        //         new OptionalConstraint(
        //             new SequenceConstraint(
        //                 new OrConstraint(
        //                     new CrumpetTerminalConstraint(CrumpetToken.PLUS),
        //                     new CrumpetTerminalConstraint(CrumpetToken.MINUS)),
        //                 new NonTerminalConstraint<MultExpressionNode>()))),
        //     GetNodeConstructor<SumExpressionNode>());
        
        // sum
        yield return new NonTerminalDefinition<SumExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<MultExpressionNode>(),
                new SequenceConstraint(
                    new OrConstraint(
                        new CrumpetTerminalConstraint(CrumpetToken.PLUS),
                        new CrumpetTerminalConstraint(CrumpetToken.MINUS)),
                    new NonTerminalConstraint<MultExpressionNode>())),
            GetNodeConstructor<SumExpressionNode>(3));
        
        // passthrough
        yield return new NonTerminalDefinition<SumExpressionNode>(
                new NonTerminalConstraint<MultExpressionNode>(),
            GetNodeConstructor<SumExpressionNode>(1));
    }
}