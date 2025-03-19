using Crumpet.Instructions;
using Crumpet.Instructions.Binary;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class SumExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public MultExpressionNode Primary { get; }
    public IEnumerable<SumExpressionNodeArgumentCollator> Arguments { get; }

    public SumExpressionNode(MultExpressionNode primary, IEnumerable<SumExpressionNodeArgumentCollator> arguments) : base(primary)
    {
        Primary = primary;
        Arguments = arguments.ToArray();

        foreach (SumExpressionNodeArgumentCollator collator in Arguments)
        {
            ImplicitChildren.Add(collator.Sugar);
            ImplicitChildren.Add(collator.Secondary);
        }
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<SumExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<MultExpressionNode>(),
                new ZeroOrMoreConstraint(new NonTerminalConstraint<SumExpressionNodeArgumentCollator>())),
            GetNodeConstructor<SumExpressionNode>());

        yield return new NonTerminalDefinition<SumExpressionNodeArgumentCollator>(
            new SequenceConstraint(
                new OrConstraint(
                    new CrumpetTerminalConstraint(CrumpetToken.PLUS),
                    new CrumpetTerminalConstraint(CrumpetToken.MINUS)),
                new NonTerminalConstraint<MultExpressionNode>()),
            GetNodeConstructor<SumExpressionNodeArgumentCollator>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Primary;

        foreach (SumExpressionNodeArgumentCollator argument in Arguments)
        {
            yield return argument.Secondary;

            // need values in reverse order so they play nicely with the later instructions (stack order)
            switch (argument.Sugar.Token.TokenId)
            {
                case CrumpetToken.PLUS:
                    yield return new MathematicalInstruction(MathematicalInstruction.Operation.ADD);
                    break;
                case CrumpetToken.MINUS:
                    yield return new MathematicalInstruction(MathematicalInstruction.Operation.SUBTRACT);
                    break;
            }
        }
    }
    
    public class SumExpressionNodeArgumentCollator : NonTerminalNode
    {
        public TerminalNode<CrumpetToken> Sugar { get; }
        public MultExpressionNode Secondary { get; }

        public SumExpressionNodeArgumentCollator(TerminalNode<CrumpetToken> sugar, MultExpressionNode secondary)
        {
            Sugar = sugar;
            Secondary = secondary;
        }
    }
}