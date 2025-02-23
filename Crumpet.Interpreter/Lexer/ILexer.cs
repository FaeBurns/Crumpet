namespace Crumpet.Interpreter.Lexer;

public interface ILexer<T> where T : Enum
{
    IEnumerable<Token<T>> Tokenize(bool includeComments = false);
}