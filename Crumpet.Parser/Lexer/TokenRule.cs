using System.Text.RegularExpressions;

namespace Crumpet.Parser.Lexer;

internal class TokenRule<T> where T : Enum
{
    public TokenRule(T tokenId, Regex regex, TokenAttribute attribute)
    {
        TokenId = tokenId;
        Regex = regex;
        Attribute = attribute;
    }

    public T TokenId { get; }
    public Regex Regex { get; }
    
    public TokenAttribute Attribute { get; }
}