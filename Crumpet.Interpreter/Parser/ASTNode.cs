using System.Reflection;
using Crumpet.Interpreter.Exceptions;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser;

public class ASTNode
{
    public NodeLocation Location { get; set; } = new NodeLocation();

    protected static ConstructorInfo GetNodeConstructor<T>() where T : ASTNode
    {
        // get first constructor
        ConstructorInfo[] constructors = typeof(T).GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        
        return constructors.FirstOrDefault()
               ?? throw new ParserSetupException(
                   ExceptionConstants.PARSER_UNKNOWN_NODE_CONSTRUCTOR.Format(typeof(T)));
    }
}