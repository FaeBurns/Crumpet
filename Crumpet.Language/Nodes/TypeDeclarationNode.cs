using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes;

public class TypeDeclarationNode : NonTerminalNode, INonTerminalNodeFactory
{
    public IdentifierNode Name { get; }
    public TypeDeclarationFieldNode[] Fields { get; }

    public TypeDeclarationNode(IdentifierNode name, IEnumerable<TypeDeclarationFieldNode> fields)
    {
        Name = name;
        Fields = fields.ToArray();
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("typeDeclaration",
            new SequenceConstraint(
                new RawTerminalConstraint("struct"),
                new NamedTerminalConstraint("identifier"),
                new RawTerminalConstraint("{"),
                new ZeroOrMoreConstraint(new NamedTerminalConstraint("typeDeclarationField")),
                new RawTerminalConstraint("}")),
            GetNodeConstructor<DeclarationNode>());
    }

    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        yield return Name;
        foreach (TypeDeclarationFieldNode node in Fields)
        {
            yield return node;
        }
    }
}