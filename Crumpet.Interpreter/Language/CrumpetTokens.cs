using Crumpet.Interpreter.Parser;
// ReSharper disable UseVerbatimString
// ReSharper disable IdentifierTypo

namespace Crumpet.Interpreter.Language;

public enum CrumpetTokens : byte
{
    [Token("\\(")]
    LPARAN = 0,
    
    [Token("\\)")]
    RPARAN = 1,
    
    [Token("\\{")]
    LBRACK = 2,
    
    [Token("\\}")]
    RBRACK = 3,
    
    [Token("\\[")]
    LINDEX = 4,
    
    [Token("\\]")]
    RINDEX = 5,
    
    [Token("toast|scrape|burn|while|for|if|else|void|int|float|string|bool|func|break|continue")]
    KEYWORDS = 6,
    
    [Token("-?[0-9]+\\.[0-9]+")]
    FLOAT = 7,
    
    [Token("-?[0-9]+")]
    INT = 8,
    
    [Token("true|false")]
    BOOL = 9,
    
    // \".*\"
    [Token("\\\".*\\\"")]
    STRING = 10,
    
    // \*|<=|<|==|!=|=|>=|>|-|\+
    [Token("\\*|<=|<|==|!=|=|>=|>|-|\\+")]
    OPERATOR = 11,
    
    [Token("\\;")]
    SEMICOLON = 12,
    
    [Token("\\s+")]
    WHITESPACE = 13,
    
    // \r?\n
    [Token("\\r?\\n", IsNewline = true)]
    NEWLINE = 14,
    
    // \/\/.*
    // is newline is also used as a comment will consume a newline
    [Token("\\/\\/.*", IsComment = true, IsNewline = true)]
    COMMENT = 15,
    
    // alphanumeric but does not start with a number
    [Token("[a-zA-Z]+[a-zA-Z0-9]*")]
    IDENTIFIER = Byte.MaxValue - 1,
}