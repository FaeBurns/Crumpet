using System.Reflection;
using Crumpet.Interpreter.Parser.Elements;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser.NodeConstraints;

public class TerminalConstraint<T> : NodeConstraint where T : Enum
{
    public T Token { get; }
    public bool IncludeInConstructor { get; }

    public TerminalConstraint(T token, bool includeInConstructor = true)
    {
        Token = token;
        IncludeInConstructor = includeInConstructor;
    }

    public override string ToString()
    {
        return Token.ToString();
    }

    public override ParserElement? WalkStream<TToken>(ObjectStream<TerminalNode<TToken>> stream, ASTNodeRegistry<TToken> registry)
    {
        using PositionSaver<TerminalNode<TToken>> positionSaver = stream.ConstrainPosition();

        TerminalNode<TToken> node = stream.ReadNext();

        // read one from stream
        if (node.Token.TokenId.Equals(Token))
        {
            // do not reset position
            positionSaver.ConsumePosition();

            // set triggered constraint - don't worry about this token being potentially re-interpreted as that will just set TriggeredConstraint again
            node.TriggeredConstraint = (this as TerminalConstraint<TToken>)!;
            
            // stream containts already constructed TerminalNode so just return that
            return node;
        }
        
        // position is reset when using exitst
        return null;
    }
}