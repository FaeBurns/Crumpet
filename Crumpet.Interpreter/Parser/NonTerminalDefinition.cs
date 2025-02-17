using System.Reflection;
using Crumpet.Interpreter.Parser.NodeConstraints;

namespace Crumpet.Interpreter.Parser;

public class NonTerminalDefinition
{
    public Type Type { get; }
    public NodeConstraint Constraint { get; }
    public ConstructorInfo Constructor { get; }

    public NonTerminalDefinition(Type type, NodeConstraint constraint, ConstructorInfo constructor)
    {
        Type = type;
        Constraint = constraint;
        Constructor = constructor;
    }
}

public class NonTerminalDefinition<T> : NonTerminalDefinition
{
    public NonTerminalDefinition(NodeConstraint constraint, ConstructorInfo constructor) : base(typeof(T), constraint, constructor)
    {
    }
}