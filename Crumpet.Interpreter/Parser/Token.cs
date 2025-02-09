namespace Crumpet.Interpreter.Parser;

public class Token<T> where T : Enum
{
    public Token(T tokenId, string value, int lineNumber, Range lineTextRange)
    {
        TokenId = tokenId;
        Value = value;
        
        LineNumber = lineNumber;
        LineTextRange = lineTextRange;
    }
    
    public T TokenId { get; }

    public string Value { get; }
    
    public Range LineTextRange { get; }
    
    public int LineNumber { get; }
}