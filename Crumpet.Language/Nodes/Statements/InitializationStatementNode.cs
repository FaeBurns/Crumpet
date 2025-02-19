using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes.Statements;

public class InitializationStatementNode : NonTerminalNode, INonTerminalNodeFactory
{
    public TypeNode Type { get; }
    public IdentifierNode Name { get; }

    public InitializationStatementNode(TypeNode type, IdentifierNode name) : base(type, name)
    {
        Type = type;
        Name = name;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<InitializationStatementNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)),
            GetNodeConstructor<InitializationStatementNode>());
    }
}