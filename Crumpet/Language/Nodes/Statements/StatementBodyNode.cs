using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;

using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public class StatementBodyNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public StatementNode[] Statements { get; }

    // ReSharper disable PossibleMultipleEnumeration
    public StatementBodyNode(IEnumerable<StatementNode> statements) : base(statements)
    {
        Statements = statements.ToArray();
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<StatementBodyNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.LBRACK),
                new ZeroOrMoreConstraint(new NonTerminalConstraint<StatementNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.RBRACK)),
            GetNodeConstructor<StatementBodyNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Statements;
    }
}