using Crumpet.Instructions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types.Templates;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public abstract class TypeNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public required string FullName { get; init; }
    public required GenericTypeArgumentListNode TypeArgs { get; init; }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<TypeNode>(
            new SequenceConstraint(
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new ZeroOrMoreConstraint(new SequenceConstraint(
                    new CrumpetRawTerminalConstraint(CrumpetToken.PERIOD),
                    new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER))),
                new NonTerminalConstraint<GenericTypeArgumentListNode>()),
            GetNodeConstructor<TypeNodeIdentifierVariant>());

        yield return new NonTerminalDefinition<TypeNode>(
            new SequenceConstraint(
                new CrumpetTerminalConstraint(CrumpetToken.KW_KNOWN_TYPE),
                new NonTerminalConstraint<GenericTypeArgumentListNode>()),
            GetNodeConstructor<TypeNodeKeywordVariant>());
    }

    public override string ToString()
    {
        return FullName;
    }

    public IEnumerable GetInstructionsRecursive()
    {
        // push the type args to the stack
        yield return TypeArgs;
        
        // then construct the type with those args
        yield return new PushTypeIdentifierConstantInstruction(FullName, TypeArgs.TypeArguments.Length, Location);
    }
}

public class TypeNodeIdentifierVariant : TypeNode
{
    public IdentifierNode[] Segments { get; }
    
    public TypeNodeIdentifierVariant(IdentifierNode firstSegment, IEnumerable<IdentifierNode> otherSegments, GenericTypeArgumentListNode typeArgs)
    {
        TypeArgs = typeArgs;
        // prevent double enumeration
        IEnumerable<IdentifierNode> identifierNodes = otherSegments as IReadOnlyList<IdentifierNode> ?? otherSegments.ToArray();
        
        // creates a temporary iterator with firstSegment at the start
        Segments = identifierNodes.Prepend(firstSegment).ToArray();
        
        // set full name as first.other1.other2
        FullName = firstSegment.Terminal + String.Concat(identifierNodes.Select(s => '.' + s.Terminal));
    }
    
    protected override IEnumerable<ASTNode> EnumerateChildrenDerived()
    {
        foreach (IdentifierNode node in Segments)
        {
            yield return node;
        }

        yield return TypeArgs;
    }
}

public class TypeNodeKeywordVariant : TypeNode
{
    public TerminalNode<CrumpetToken> Keyword { get; }

    public TypeNodeKeywordVariant(TerminalNode<CrumpetToken> keyword, GenericTypeArgumentListNode typeArgs)
    {
        Keyword = keyword;
        TypeArgs = typeArgs;
        FullName = keyword.Terminal;
    }

    protected override IEnumerable<ASTNode?> EnumerateChildrenDerived()
    {
        yield return Keyword;
        yield return TypeArgs;
    }
}