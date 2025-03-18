using Crumpet.Interpreter.Instructions;
using Lexer;
using Parser;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class IdentifierNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>
{
    private readonly Token<CrumpetToken> m_token;

    public IdentifierNode(Token<CrumpetToken> token) : base(token)
    {
        m_token = token;
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.IDENTIFIER, GetNodeConstructor<IdentifierNode>());
    }
}