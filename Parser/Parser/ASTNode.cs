using System.Reflection;
using Parser.Elements;
using Shared;
using Shared.Exceptions;

namespace Parser;

public abstract class ASTNode : ParserElement
{
    public SourceLocation Location { get; internal set; } = new SourceLocation();

    protected static ConstructorInfo GetNodeConstructor<T>() where T : ASTNode
    {
        // get first constructor
        ConstructorInfo[] constructors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        // but filter to only apply to constructors with at least one argument
        return constructors.FirstOrDefault(c => c.GetParameters().Length > 0)
               ?? throw new ParserSetupException(
                   ExceptionConstants.PARSER_UNKNOWN_NODE_CONSTRUCTOR.Format(typeof(T)));
    }

    protected static ConstructorInfo GetNodeConstructor<T>(int paramCount) where T : ASTNode
    {
        // get first constructor with count
        ConstructorInfo[] constructors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        return constructors.FirstOrDefault(c => c.GetParameters().Length == paramCount)
               ?? throw new ParserSetupException(
                   ExceptionConstants.PARSER_UNKNOWN_NODE_CONSTRUCTOR.Format(typeof(T)));
    }
}