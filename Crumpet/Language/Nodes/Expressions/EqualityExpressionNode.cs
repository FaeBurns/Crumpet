using System.Diagnostics;
using Crumpet.Instructions.Binary;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class EqualityExpressionNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public RelationExpressionNode Primary { get; }
    public IEnumerable<EqualityExpressionNodeArgumentCollator> Arguments { get; }

    public EqualityExpressionNode(RelationExpressionNode primary, IEnumerable<EqualityExpressionNodeArgumentCollator> arguments) : base(primary)
    {
        Primary = primary;
        Arguments = arguments.ToArray();

        foreach (EqualityExpressionNodeArgumentCollator collator in Arguments)
        {
            ImplicitChildren.Add(collator.Sugar);
            ImplicitChildren.Add(collator.Secondary);
        }
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<EqualityExpressionNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<RelationExpressionNode>(),
                new ZeroOrMoreConstraint(new NonTerminalConstraint<EqualityExpressionNodeArgumentCollator>())),
            GetNodeConstructor<EqualityExpressionNode>());

        yield return new NonTerminalDefinition<EqualityExpressionNodeArgumentCollator>(
            new SequenceConstraint(
                new OrConstraint(
                    new CrumpetTerminalConstraint(CrumpetToken.EQUALS_EQUALS),
                    new CrumpetTerminalConstraint(CrumpetToken.NOT_EQUALS)),
                new NonTerminalConstraint<RelationExpressionNode>()),
            GetNodeConstructor<EqualityExpressionNodeArgumentCollator>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Primary;

        foreach (EqualityExpressionNodeArgumentCollator argument in Arguments)
        {
            yield return argument.Secondary;
            yield return new EqualityInstruction(argument.Sugar.Token.TokenId == CrumpetToken.NOT_EQUALS, Location);
        }
    }

    public class EqualityExpressionNodeArgumentCollator : NonTerminalNode
    {
        public TerminalNode<CrumpetToken> Sugar { get; }
        public RelationExpressionNode Secondary { get; }

        public EqualityExpressionNodeArgumentCollator(TerminalNode<CrumpetToken> sugar, RelationExpressionNode secondary)
        {
            Sugar = sugar;
            Secondary = secondary;
        }
    }
}