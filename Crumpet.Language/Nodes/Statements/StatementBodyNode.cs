using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public class StatementBodyNode : NonTerminalNode, INonTerminalNodeFactory
{
    public StatementNode[] Statements { get; }
    
    // ReSharper disable PossibleMultipleEnumeration
    public StatementBodyNode(IEnumerable<StatementNode> statements) : base(statements)
    {
        Statements = statements.ToArray();
    }   
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("statementBody",
            new SequenceConstraint(
                new RawTerminalConstraint("{"),
                new ZeroOrMoreConstraint(new NonTerminalConstraint("statement")),
                new RawTerminalConstraint("}")),
            GetNodeConstructor<StatementBodyNode>());
    }
}