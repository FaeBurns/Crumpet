using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

/// <summary>
/// A node that defines the names of the generic type arguments a type or class contains
/// </summary>
public class GenericTypeDeclarationListNode : NonTerminalNode, INonTerminalNodeFactory
{
    public IdentifierNode[] GenericTypeNames { get; }

    public GenericTypeDeclarationListNode(IEnumerable<IdentifierNode>? genericTypeNames)
    {
        GenericTypeNames = genericTypeNames?.ToArray() ?? Array.Empty<IdentifierNode>();
        ImplicitChildren.AddRange(GenericTypeNames);
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<GenericTypeDeclarationListNode>(
            new ZeroOrMoreConstraint(new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.LESS),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.GREATER))),
            GetNodeConstructor<GenericTypeDeclarationListNode>());
    }
}