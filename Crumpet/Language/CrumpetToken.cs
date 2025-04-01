// ReSharper disable UseVerbatimString
// ReSharper disable IdentifierTypo

using Lexer;

namespace Crumpet.Language;

public enum CrumpetToken : byte
{
    // \/\/.*
    [Token("\\/\\/.*", IsComment = true)]
    COMMENT,

    [Token("\\(")]
    LPARAN,

    [Token("\\)")]
    RPARAN,

    [Token("\\{")]
    LBRACK,

    [Token("\\}")]
    RBRACK,

    [Token("\\[")]
    LINDEX,

    [Token("\\]")]
    RINDEX,

    [Token("while")]
    KW_WHILE,
    
    [Token("for")]
    KW_FOR,

    [Token("if")]
    KW_IF,

    [Token("else")]
    KW_ELSE,

    [Token("void|string|int|float|bool")]
    KW_KNOWN_TYPE,

    [Token("struct")]
    KW_STRUCT,

    [Token("func")]
    KW_FUNC,

    [Token("break")]
    KW_BREAK,

    [Token("continue")]
    KW_CONTINUE,

    [Token("return")]
    KW_RETURN,
    
    [Token("try")]
    KW_TRY,
    
    [Token("catch")]
    KW_CATCH,
    
    [Token("include")]
    KW_INCLUDE,

    [Token("-?[0-9]+\\.[0-9]+")]
    FLOAT,

    [Token("-?[0-9]+")]
    INT,

    [Token("true|false")]
    BOOL,

    // "((\\")|[^"])*" - with " escape
    // "[^"]*" - no " escape
    [Token(@"""[^""]*""")]
    STRING,
    
    // allow either case
    [Token(("NULL|null"))]
    NULL,

    [Token("\\^")]
    XOR,

    [Token("\\*")]
    MULTIPLY,

    [Token("<=")]
    LESS_OR_EQUAL,

    [Token("<")]
    LESS,

    [Token("==")]
    EQUALS_EQUALS,

    [Token("=")]
    EQUALS,

    [Token("!=")]
    NOT_EQUALS,

    [Token("!")]
    NOT,

    [Token(">=")]
    GREATER_OR_EQUAL,

    [Token(">")]
    GREATER,

    [Token("--")]
    MINUSMINUS,
    
    [Token("-")]
    MINUS,

    [Token("\\+\\+")]
    PLUSPLUS,
    
    [Token("\\+")]
    PLUS,

    [Token("\\/")]
    DIVIDE,
    
    [Token("\\%")]
    MODULO,

    [Token(",")]
    COMMA,

    [Token("\\.")]
    PERIOD,
    
    [Token("\\-\\>")]
    DEREFERENCE,

    [Token("\\;")]
    SEMICOLON,

    [Token("\\&\\&")]
    AND_AND,

    [Token("\\|\\|")]
    OR,

    [Token("\\&")]
    REFERENCE,
    
    [Token("\\#")]
    PREPROCESSOR,

    // cannot use \s as newline will not be detected then
    [Token("[\\t ]+")]
    WHITESPACE,

    // \r?\n
    [Token("\\r?\\n", IsNewline = true)]
    NEWLINE,

    // alphanumeric but does not start with a number
    [Token("[a-zA-Z_]+[a-zA-Z0-9_]*")]
    IDENTIFIER = Byte.MaxValue - 1,
}