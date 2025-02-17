using Crumpet.Interpreter.Lexer;
using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class IntLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>
{
    public int IntLiteral { get; }
    
    public IntLiteralNode(Token<CrumpetToken> token) : base(token)
    {
        IntLiteral = Convert.ToInt32(token.Value);
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.INT, GetNodeConstructor<IntLiteralNode>());
    }
}