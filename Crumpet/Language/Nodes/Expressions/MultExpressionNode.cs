using Crumpet.Instructions;
using Crumpet.Instructions.Binary;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class MultExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public UnaryExpressionNode Primary { get; }
    public IEnumerable<MultExpressionNodeArgumentCollator> Arguments { get; }

    public MultExpressionNode(UnaryExpressionNode primary, IEnumerable<MultExpressionNodeArgumentCollator> arguments) : base(primary)
    {
        Primary = primary;
        Arguments = arguments.ToArray();

        foreach (MultExpressionNodeArgumentCollator collator in Arguments)
        {
            ImplicitChildren.Add(collator.Sugar);
            ImplicitChildren.Add(collator.Secondary);
        }
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        // mult
        yield return new NonTerminalDefinition<MultExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<UnaryExpressionNode>(),
                new ZeroOrMoreConstraint(new NonTerminalConstraint<MultExpressionNodeArgumentCollator>())),
            GetNodeConstructor<MultExpressionNode>());

        // arguments
        yield return new NonTerminalDefinition<MultExpressionNodeArgumentCollator>(
            new SequenceConstraint(
                new OrConstraint(
                    new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY),
                    new CrumpetTerminalConstraint(CrumpetToken.DIVIDE)),
                new NonTerminalConstraint<UnaryExpressionNode>()),
            GetNodeConstructor<MultExpressionNodeArgumentCollator>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Primary;
        
        foreach (MultExpressionNodeArgumentCollator argument in Arguments)
        {
            yield return argument.Secondary;

            switch (argument.Sugar.Token.TokenId)
            {
                case CrumpetToken.MULTIPLY:
                    yield return new MathematicalInstruction(MathematicalInstruction.Operation.MULTIPLY, Location);
                    break;
                case CrumpetToken.DIVIDE:
                    yield return new MathematicalInstruction(MathematicalInstruction.Operation.DIVIDE, Location);
                    break;
            }
        }
    }

    public class MultExpressionNodeArgumentCollator : NonTerminalNode
    {
        public TerminalNode<CrumpetToken> Sugar { get; }
        public UnaryExpressionNode Secondary { get; }

        public MultExpressionNodeArgumentCollator(TerminalNode<CrumpetToken> sugar, UnaryExpressionNode secondary)
        {
            Sugar = sugar;
            Secondary = secondary;
        }
    }
}