﻿using Crumpet.Interpreter.Lexer;
using Crumpet.Interpreter.Parser.NodeConstraints;

namespace Crumpet.Interpreter.Parser.Nodes;

public class TerminalNode<T> : ASTNode where T : Enum
{
    public Token<T> Token { get; }
    public string Terminal { get; }

    public TerminalNode(Token<T> token)
    {
        Token = token;
        Terminal = token.Value;
    }

    public TerminalConstraint<T> TriggeredConstraint { get; internal set; } = null!;

    public override string ToString()
    {
        return $"{{{Token.TokenId}}}:{{{Terminal}}}";
    }

    public override IEnumerable<object> TransformForConstructor()
    {
        if (TriggeredConstraint.IncludeInConstructor)
            return [this];
        return Array.Empty<object>();
    }
}