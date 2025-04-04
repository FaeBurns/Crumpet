using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class GenericTypeArgumentListNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public TypeNode[] TypeArguments { get; }

    public GenericTypeArgumentListNode(TypeNode? first, IEnumerable<TypeNode>? remaining)
    {
        List<TypeNode> typeArguments = new List<TypeNode>(remaining ?? Enumerable.Empty<TypeNode>());
        if (first != null)
            typeArguments.Insert(0, first);

        TypeArguments = typeArguments.ToArray();
        ImplicitChildren.AddRange(TypeArguments);
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<GenericTypeArgumentListNode>(
            new OptionalConstraint(new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.LESS),
                new NonTerminalConstraint<TypeNode>(),
                new ZeroOrMoreConstraint(new SequenceConstraint(
                    new CrumpetRawTerminalConstraint(CrumpetToken.COMMA),
                    new NonTerminalConstraint<TypeNode>())),
                new CrumpetRawTerminalConstraint(CrumpetToken.GREATER)
                )),
            GetNodeConstructor<GenericTypeArgumentListNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return TypeArguments;
    }
}