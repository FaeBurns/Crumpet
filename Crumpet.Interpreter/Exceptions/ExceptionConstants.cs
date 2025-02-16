namespace Crumpet.Interpreter.Exceptions;

public static class ExceptionConstants
{
    public const string PARSER_UNKNOWN_NODE_CONSTRUCTOR = "Could not find node constructor on type {1}";
    public const string PARSER_UNKOWN_TERMINAL = "Could not find terminal named \"{0}\"";
    public const string PARSER_UNKOWN_NONTERMINAL = "Could not find non-terminal named \"{0}\"";
    public const string PARSER_INVALID_FACTORY_ELEMENT = "Type returned from factory \"{0}\" does not implement required interface \"{1}\"";
}