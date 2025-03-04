namespace Crumpet;

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
    public const string VARIABLE_ALREADY_EXISTS = "Variable \"{0}\" already exists";
    public const string VARIABLE_NOT_FOUND = "Variable \"{0}\" not found";
    public const string POP_SCOPE_FAILED = "Could not pop scope. Already at top level";
    public const string KEY_NOT_FOUND = "Key \"{0}\" not found";
    public const string KEY_ALREADY_EXISTS = "Key \"{0}\" already exists";
    public const string CREATE_PLACEHOLDER_TYPE = "Cannot create an instance of a type placeholder";
    public const string REPLACING_NON_PLACEHOLDER_TYPE = "Cannot replace a non-placeholder type";
    public const string PLACEHOLDER_STILL_PRESENT = "Placeholder of type \"{0}\" still present.";
}