using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Statements;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes;

public class FunctionDeclarationNode : NonTerminalNode, INonTerminalNodeFactory
{
    public TypeNode Type { get; }
    public IdentifierNode Name { get; }
    public ParameterListNode Parameters { get; }
    public StatementBodyNode StatementBody { get; }

    public FunctionDeclarationNode(TypeNode type, IdentifierNode name, ParameterListNode parameters, StatementBodyNode statementBody) : base(type, name, parameters, statementBody)
    {
        Type = type;
        Name = name;
        Parameters = parameters;
        StatementBody = statementBody;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("functionDeclaration",
            new SequenceConstraint(
                new RawTerminalConstraint("func"),
                new NonTerminalConstraint("type"),
                new NamedTerminalConstraint("identifier"),
                new RawTerminalConstraint("("),
                new ZeroOrMoreConstraint(new NamedTerminalConstraint("parameterList")),
                new RawTerminalConstraint(")"),
                new NamedTerminalConstraint("statementBody")),
            GetNodeConstructor<FunctionDeclarationNode>());
    }
}