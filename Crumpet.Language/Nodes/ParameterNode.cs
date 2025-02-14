using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes;

public class ParameterNode : NonTerminalNode, INonTerminalNodeFactory
{
    public TypeNode Type { get; }
    public IdentifierNode Name { get; }

    public ParameterNode(TypeNode type, IdentifierNode name) : base(type, name)
    {
        Type = type;
        Name = name;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("parameter",
            new SequenceConstraint(
                new NonTerminalConstraint("type"),
                new NamedTerminalConstraint("identifier")), 
            GetNodeConstructor<ParameterNode>());
    }
}