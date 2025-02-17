using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;

namespace Crumpet.Language.Nodes.Statements;

public class InitializationStatementNode : NonTerminalNode, INonTerminalNodeFactory
{
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<InitializationStatementNode>(
            new SequenceConstraint(
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)),
            GetNodeConstructor<InitializationStatementNode>());
    }
}