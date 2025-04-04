using System.Diagnostics;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public abstract class ParameterNode : NonTerminalNode, INonTerminalNodeFactory
{
    public TypeNode Type { get; }
    public TerminalNode<CrumpetToken>? ModifierSugar { get; }
    public IdentifierNode Name { get; }
    
    protected ParameterNode(TypeNode type, TerminalNode<CrumpetToken>? modifierSugar, IdentifierNode name) : base(type, modifierSugar, name)
    {
        Type = type;
        ModifierSugar = modifierSugar;
        Name = name;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<ParameterNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)),
            GetNodeConstructor<ParameterNodeBasicVariant>());

        yield return new NonTerminalDefinition<ParameterNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.LINDEX),
                new CrumpetRawTerminalConstraint(CrumpetToken.RINDEX),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)),
            GetNodeConstructor<ParameterNodeArrayVariant>());
    }
    
    public VariableModifier GetModifier(TerminalNode<CrumpetToken>? sugar)
    {
        if (sugar is null)
            return VariableModifier.COPY;

        // re-use multiply as multiple identical tokens cannot exist
        if (sugar.Token.TokenId == CrumpetToken.MULTIPLY)
            return VariableModifier.POINTER;

        throw new UnreachableException();
    }
}

public class ParameterNodeBasicVariant : ParameterNode
{
    public ParameterNodeBasicVariant(TypeNode type, TerminalNode<CrumpetToken>? modifierSugar, IdentifierNode name) : base(type, modifierSugar, name)
    {
    }
}

public class ParameterNodeArrayVariant : ParameterNode
{
    public ParameterNodeArrayVariant(TypeNode type, TerminalNode<CrumpetToken>? modifierSugar, IdentifierNode name) : base(type, modifierSugar, name)
    {
    }
}