using Shared;

namespace Lexer;

public class Token<T> where T : Enum
{
    public Token(T tokenId, string value, SourceLocation location)
    {
        TokenId = tokenId;
        Value = value;

        Location = location;
    }

    public T TokenId { get; }

    public string Value { get; }

    public SourceLocation Location { get; }
}