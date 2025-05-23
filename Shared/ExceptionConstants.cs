﻿namespace Shared;

public static class ExceptionConstants
{
    public const string PARSER_UNKNOWN_NODE_CONSTRUCTOR = "Could not find node constructor on type \"{0}\"";
    public const string PARSER_UNKOWN_TERMINAL = "Could not find terminal named \"{0}\"";
    public const string PARSER_UNKOWN_NONTERMINAL = "Could not find non-terminal named \"{0}\"";
    public const string PARSER_INVALID_FACTORY_ELEMENT = "Type returned from factory \"{0}\" does not implement required interface \"{1}\"";
    public const string NO_NODES_REGISTERED = "No nodes registered when trying to build node tree";
    public const string INVALID_NODE_NAME = "Could not find a node with name \"{0}\"";
    public const string MISSING_TERMINAL_NODE = "Could not find a node for the terminal token \"{0}\". This terminal has not been referenced in the expression tree";
    public const string NODE_CONSTRUCTOR_FAILED = "Failed invoking constructor for node \"{0}\"";
    public const string VARIABLE_ALREADY_EXISTS = "Variable \"{0}\" already exists";
    public const string VARIABLE_NOT_FOUND = "Variable \"{0}\" not found";
    public const string POP_SCOPE_FAILED = "Could not pop scope. Already at top level";
    public const string KEY_NOT_FOUND = "Key \"{0}\" not found";
    public const string KEY_ALREADY_EXISTS = "Key \"{0}\" already exists";
    public const string CREATE_PLACEHOLDER_TYPE = "Cannot create an instance of a type placeholder";
    public const string REPLACING_NON_PLACEHOLDER_TYPE = "Cannot replace a non-placeholder type";
    public const string PLACEHOLDER_STILL_PRESENT = "Placeholder of type \"{0}\" still present";
    public const string INVALID_ARGUMENT_COUNT = "Invalid argument count. Expected {0} but received {1}";
    public const string INVALID_ARGUMENT_TYPE = "Invalid argument type at index {0}. Expected {1} but received {2}";
    public const string CONVERSION_TARGET_INVALID = "Cannot convert type \"{0}\" to \"{1}\"";
    public const string CONVERT_REFERENCE_ASSIGN = "Cannot convert while assigning a reference or pointer";
    public const string CREATE_INSTANCE_INVALID_INITIAL_VALUE = "Initial value given to instance reference is not valid for this type";
    public const string EXPECTED_VARIABLE_NOT_VALUE = "Failed to set variable as value given was not a Variable and was instead a value";
    public const string VARIABLE_STACK_POP_WHILE_EMPTY = "Tried to pop a variable from the stack while it was empty";
    public const string VARIABLE_STACK_PEEK_WHILE_EMPTY = "Tried to peek a variable from the stack while it was empty";
    public const string VARIABLE_STACK_PEEK_INSUFFICIENT_COUNT = "Tried to peek {0} variables from the stack while there were {1} variables";
    public const string INVALID_TYPE = "Invalid type detected during operation. Expected \"{0}\" received \"{1}\"";
    public const string FUNCTION_NOT_FOUND = "Function \"{0}\" not found with parameters ({1})";
    public const string UNKOWN_TYPE = "Could not find type named \"{0}\"";
    public const string UNKOWN_RETURN_TYPE = "Could not find type for return named \"{0}\"";
    public const string INVALID_FUNCTION_CALL_EXPRESSION_TYPE = "Invalid expression given to function call";
    public const string VALUE_SEARCH_FAILED = "Failed to find target field \"{0}\" in value.";
    public const string NO_EXECUTING_UNIT = "No unit to execute";
    public const string PASSING_RETURNABLE_UNIT = "Cannot pass ";
    public const string UNCAUGHT_RUNTIME_EXCEPTION = "An Uncaught Exception occured while running the program";
    public const string COULD_NOT_FIND_INSTRUCTION_TO_JUMP_TO = "Failed to find instruction of type \"{0}\" to jump to";
    public const string MODIFIER_MISMATCH = "Variable modifier mismatch. Expected \"{0}\" received \"{1}\"";
    public const string INVALID_RETURN_TYPE = "Could not convert returned value to the expected type. Expected \"{0}\" received \"{1}\"";
    public const string PARSE_FILE_NOT_FOUND = "Failed to parse file \"{0}\". File was not found.";
    public const string FAILED_TO_WALK_TREE = "Failed to walk full node tree. Ended on token \"{0}\" at location \"{1}\"";
    public const string PARSE_INNER_EXCEPTION = "Failed to parse file \"{0}\". File contained errors on parse";
    public const string FILE_ENCODING_INVALID = "Incorrect file format. Expected \"{0}\" received \"{1}\"";
    public const string MISSING_RETURN_STATEMENT = "Function returning value of type \"{0}\" Expects a return statement but none was present";
    public const string TYPE_RESOLVE_GENERIC_ARG_COUNT_MISMATCH = "Failed to resolve type. Expected {0} generic arguments, Received {1}";
    public const string UNRESOLVED_GENERIC_TYPE = "Generic used while unresolved";
    public const string GENERIC_ARGUMENT_COUNT_MISMATCH = "Expected {0} generic arguments, Received {1}";
    public const string GENERIC_NOT_FOUND = "No generic parameter matching \"{0}\" found.";
    public const string GENERIC_ARGUMENT_OUT_OF_RANGE = "Generic argument out of range. Range 0-{0}, received index {1}";
    public const string INVALID_ENTRY_POINT_ARGUMENT_TYPE = "Invalid argument type \"{0}\" for an entry point function";
}
