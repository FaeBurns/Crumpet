namespace Crumpet.Interpreter.Exceptions;

public static class ExceptionConstants
{
    public const string PARSER_UNKNOWN_NODE_CONSTRUCTOR = "Could not find node constructor on type {1}";
    public const string PARSER_UNKOWN_TERMINAL = "Could not find terminal named \"{0}\"";
    public const string PARSER_UNKOWN_NONTERMINAL = "Could not find non-terminal named \"{0}\"";
    public const string PARSER_INVALID_FACTORY_ELEMENT = "Type returned from factory \"{0}\" does not implement required interface \"{1}\"";
    public const string NO_NODES_REGISTERED = "No nodes registered when trying to build node tree";
    public const string INVALID_NODE_NAME = "Could not find a node with name \"{0}\"";
    public const string MISSING_TERMINAL_NODE = "Could not find a node for the terminal token \"{0}\". This terminal has not been referenced in the expression tree.";
    public const string NODE_CONSTRUCTOR_FAILED = "Failed invoking constructor for node \"{0}\"";
}