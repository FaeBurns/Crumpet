using Crumpet.Parser;
using Crumpet.Parser.Lexer;
using Crumpet.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class StringLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>
{
    public StringLiteralNode(Token<CrumpetToken> token) : base(token)
    {
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        // \".*\"
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.STRING, GetNodeConstructor<StringLiteralNode>());
    }
}