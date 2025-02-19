using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Statements;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes;

public class FunctionDeclarationNode : NonTerminalNode, INonTerminalNodeFactory
{
    public TypeNode Type { get; }
    public IdentifierNode Name { get; }
    public ParameterListNode Parameters { get; }
    public StatementBodyNode StatementBody { get; }

    public FunctionDeclarationNode(TypeNode type, IdentifierNode name, ParameterListNode? parameters, StatementBodyNode statementBody) : base(type, name, parameters, statementBody)
    {
        Type = type;
        Name = name;
        StatementBody = statementBody;

        // don't know why this thinks it'll never be null when it's cleary annotated to be
        Parameters = parameters ?? new ParameterListNode();
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<FunctionDeclarationNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_FUNC),
                new NonTerminalConstraint<TypeNode>(),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new OptionalConstraint(new NonTerminalConstraint<ParameterListNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN),
                new NonTerminalConstraint<StatementBodyNode>()),
            GetNodeConstructor<FunctionDeclarationNode>());
    }
}