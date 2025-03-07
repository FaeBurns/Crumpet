using Crumpet.Parser;
using Crumpet.Parser.Lexer;
using Crumpet.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class FloatLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>
{
    public float FloatLiteral { get; }
    
    public FloatLiteralNode(Token<CrumpetToken> token) : base(token)
    {
        FloatLiteral = Convert.ToSingle(token.Value);
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.FLOAT, GetNodeConstructor<FloatLiteralNode>());
    }
}