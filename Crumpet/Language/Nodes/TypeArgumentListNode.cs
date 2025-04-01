using Crumpet.Instructions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class TypeArgumentListNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public TypeNode[] Types { get; }
    
    public TypeArgumentListNode(TypeNode first, IEnumerable<TypeNode> secondaries)
    {
        Types = secondaries.Prepend(first).ToArray();

        foreach (TypeNode type in Types)
        {
            ImplicitChildren.Add(type);
        }
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<TypeArgumentListNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.LESS),
                new SequenceConstraint(
                    new NonTerminalConstraint<TypeNode>(),
                    new ZeroOrMoreConstraint(new SequenceConstraint(
                        new CrumpetRawTerminalConstraint(CrumpetToken.COMMA),
                        new NonTerminalConstraint<TypeNode>()))
                    ),
                new CrumpetRawTerminalConstraint(CrumpetToken.GREATER)
                ),
            GetNodeConstructor<TypeArgumentListNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        foreach (TypeNode type in Types)
        {
            yield return new PushTypeIdentifierConstantInstruction(type.FullName, Location);
        }
    }
}