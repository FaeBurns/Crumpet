using System.Diagnostics;
using Crumpet.Instructions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Statements;

public abstract class InitializationStatementNode : NonTerminalNode, INonTerminalNodeFactory
{
    protected InitializationStatementNode(params IEnumerable<ASTNode?> implicitChildren) : base(implicitChildren)
    {
        
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<InitializationStatementNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.REFERENCE)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)),
            GetNodeConstructor<InitializationStatementNodeBasicVariant>());

        yield return new NonTerminalDefinition<InitializationStatementNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.REFERENCE)),
                new CrumpetRawTerminalConstraint(CrumpetToken.LINDEX),
                new CrumpetRawTerminalConstraint(CrumpetToken.RINDEX),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.REFERENCE)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)),
            GetNodeConstructor<InitializationStatementNodeArrayVariant>());
    }

    protected VariableModifier GetModifier(TerminalNode<CrumpetToken>? sugar)
    {
        if (sugar is null)
            return VariableModifier.COPY;

        // re-use multiply as multiple identical tokens cannot exist
        if (sugar.Token.TokenId == CrumpetToken.REFERENCE)
            return VariableModifier.POINTER;

        throw new UnreachableException();
    }
}

public class InitializationStatementNodeBasicVariant : InitializationStatementNode, IInstructionProvider
{
    public TypeNode Type { get; }
    public TerminalNode<CrumpetToken>? ModifierSugar { get; }
    public IdentifierNode Name { get; }
    
    public InitializationStatementNodeBasicVariant(TypeNode type, TerminalNode<CrumpetToken>? modifierSugar, IdentifierNode name) : base(type, modifierSugar, name)
    {
        Type = type;
        ModifierSugar = modifierSugar;
        Name = name;
    }
    
    public IEnumerable GetInstructionsRecursive()
    {
        yield return new CreateVariableInstruction(Name.Terminal, Type.FullName, GetModifier(ModifierSugar), false);
    }
}

public class InitializationStatementNodeArrayVariant : InitializationStatementNode, IInstructionProvider
{
    public TypeNode Type { get; }
    public TerminalNode<CrumpetToken>? ArrayModifierSugar { get; }
    public TerminalNode<CrumpetToken>? ModifierSugar { get; }
    public IdentifierNode Name { get; }
    
    public InitializationStatementNodeArrayVariant(TypeNode type, TerminalNode<CrumpetToken>? modifierSugar, TerminalNode<CrumpetToken>? arrayModifierSugar, IdentifierNode name) : base(type, modifierSugar, arrayModifierSugar, name)
    {
        Type = type;
        ArrayModifierSugar = arrayModifierSugar;
        ModifierSugar = modifierSugar;
        Name = name;
    }
    
    public IEnumerable GetInstructionsRecursive()
    {
        yield return new CreateVariableInstruction(Name.Terminal, Type.FullName, GetModifier(ModifierSugar), true, GetModifier(ArrayModifierSugar));
    }
}