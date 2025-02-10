namespace Crumpet.Interpreter.Parser;

public interface ILexer<T> where T : Enum
{
    IEnumerable<Token<T>> Tokenize();
}