using System.Diagnostics;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public abstract class ParameterNode : NonTerminalNode, INonTerminalNodeFactory
{
    protected ParameterNode(params IEnumerable<ASTNode?> implicitChildren) : base(implicitChildren)
    {
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ParameterNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.REFERENCE)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)),
            GetNodeConstructor<ParameterNodeBasicVariant>());

        yield return new NonTerminalDefinition<ParameterNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.LINDEX),
                new CrumpetRawTerminalConstraint(CrumpetToken.RINDEX),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.REFERENCE)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)),
            GetNodeConstructor<ParameterNodeArrayVariant>());
    }
    
    public VariableModifier GetModifier(TerminalNode<CrumpetToken>? sugar)
    {
        if (sugar is null)
            return VariableModifier.COPY;

        // re-use multiply as multiple identical tokens cannot exist
        if (sugar.Token.TokenId == CrumpetToken.REFERENCE)
            return VariableModifier.POINTER;

        throw new UnreachableException();
    }
}

public class ParameterNodeBasicVariant : ParameterNode
{
    public TypeNode Type { get; }
    public TerminalNode<CrumpetToken>? ModifierSugar { get; }
    public IdentifierNode Name { get; }
    public ParameterNodeBasicVariant(TypeNode type, TerminalNode<CrumpetToken>? modifierSugar, IdentifierNode name) : base(type, modifierSugar, name)
    {
        Type = type;
        ModifierSugar = modifierSugar;
        Name = name;
    }
}

public class ParameterNodeArrayVariant : ParameterNode
{
    public TypeNode Type { get; }
    public TerminalNode<CrumpetToken>? ModifierSugar { get; }
    public IdentifierNode Name { get; }
    public ParameterNodeArrayVariant(TypeNode type, TerminalNode<CrumpetToken>? modifierSugar, IdentifierNode name) : base(type, modifierSugar, name)
    {
        Type = type;
        ModifierSugar = modifierSugar;
        Name = name;
    }
}