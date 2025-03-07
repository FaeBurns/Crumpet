namespace Crumpet.Parser.Lexer;

public interface ILexer<T> where T : Enum
{
    IEnumerable<Token<T>> Tokenize(bool includeComments = false);
}