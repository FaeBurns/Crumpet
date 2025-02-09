namespace Crumpet.Interpreter.Parser;

public interface ILexer<T> where T : Enum
{
    IEnumerator<Token<T>> Tokenize();
}