using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;

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
        yield return new NonTerminalDefinition<StatementBodyNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.RBRACK),
                new ZeroOrMoreConstraint(new NonTerminalConstraint<StatementNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.LBRACK)),
            GetNodeConstructor<StatementBodyNode>());
    }
}