using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public class InitializationStatementNode : NonTerminalNode, INonTerminalNodeFactory
{
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("initializationStatement",
            new SequenceConstraint(
                new NamedTerminalConstraint("identifier"),
                new NamedTerminalConstraint("identifier"),
                new RawTerminalConstraint(";")),
            GetNodeConstructor<InitializationStatementNode>());
    }
}