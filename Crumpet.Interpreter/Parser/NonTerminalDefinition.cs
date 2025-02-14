using System.Reflection;
using Crumpet.Interpreter.Parser.NodeConstraints;

namespace Crumpet.Interpreter.Parser;

public sealed class NonTerminalDefinition
{
    public string Name { get; }
    public NodeConstraint Constraint { get; }
    public ConstructorInfo Constructor { get; }

    public NonTerminalDefinition(string name, NodeConstraint constraint, ConstructorInfo constructor)
    {
        Name = name;
        Constraint = constraint;
        Constructor = constructor;
    }
}