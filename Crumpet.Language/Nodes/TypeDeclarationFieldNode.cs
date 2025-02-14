using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes;

public class TypeDeclarationFieldNode : NonTerminalNode, INonTerminalNodeFactory
{
    public TypeNode Type { get; }
    public IdentifierNode Name { get; }

    public TypeDeclarationFieldNode(TypeNode type, IdentifierNode name) : base(type, name)
    {
        Type = type;
        Name = name;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("typeDeclarationField",
            new SequenceConstraint(
                new NonTerminalConstraint("type"), 
                new NamedTerminalConstraint("identifier"), 
                new RawTerminalConstraint(";")),
            GetNodeConstructor<TypeDeclarationFieldNode>());
    }
}