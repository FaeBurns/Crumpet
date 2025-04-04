using System.Diagnostics;
using Crumpet.Instructions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Expressions;
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
        // shared between all variants
        NodeConstraint assignmentConstraint = new OptionalConstraint(new SequenceConstraint(
            new CrumpetRawTerminalConstraint(CrumpetToken.EQUALS),
            new NonTerminalConstraint<AssignmentExpressionNode>()));
        
        yield return new NonTerminalDefinition<InitializationStatementNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                assignmentConstraint),
            GetNodeConstructor<InitializationStatementNodeBasicVariant>());

        yield return new NonTerminalDefinition<InitializationStatementNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new CrumpetRawTerminalConstraint(CrumpetToken.LINDEX),
                new CrumpetRawTerminalConstraint(CrumpetToken.RINDEX),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                assignmentConstraint),
            GetNodeConstructor<InitializationStatementNodeArrayVariant>());
    }

    protected VariableModifier GetModifier(TerminalNode<CrumpetToken>? sugar)
    {
        if (sugar is null)
            return VariableModifier.COPY;

        // re-use multiply as multiple identical tokens cannot exist
        if (sugar.Token.TokenId == CrumpetToken.MULTIPLY)
            return VariableModifier.POINTER;

        throw new UnreachableException();
    }
}

public class InitializationStatementNodeBasicVariant : InitializationStatementNode, IInstructionProvider
{
    public TypeNode Type { get; }
    public TerminalNode<CrumpetToken>? ModifierSugar { get; }
    public IdentifierNode Name { get; }
    public AssignmentExpressionNode? Assignment { get; }

    public InitializationStatementNodeBasicVariant(
        TypeNode type,
        TerminalNode<CrumpetToken>? modifierSugar,
        IdentifierNode name,
        AssignmentExpressionNode? assignment)
        : base(type, modifierSugar, name, assignment)
    {
        Type = type;
        ModifierSugar = modifierSugar;
        Name = name;
        Assignment = assignment;
    }
    
    public IEnumerable GetInstructionsRecursive()
    {
        yield return Type;
        yield return new CreateVariableInstruction(Name.Terminal, GetModifier(ModifierSugar), false, Location);

        if (Assignment != null)
        {
            // target
            yield return new PushNamedVariableInstruction(Name.Terminal, Location);
            // source
            yield return Assignment;
            // assign
            yield return new AssignVariableInstruction(Location);
        }
    }
}

public class InitializationStatementNodeArrayVariant : InitializationStatementNode, IInstructionProvider
{
    public TypeNode Type { get; }
    public TerminalNode<CrumpetToken>? ModifierSugar { get; }
    public IdentifierNode Name { get; }
    public AssignmentExpressionNode? Assignment { get; }

    public InitializationStatementNodeArrayVariant(
        TypeNode type,
        TerminalNode<CrumpetToken>? modifierSugar, 
        IdentifierNode name, 
        AssignmentExpressionNode? assignment) 
        : base(type, modifierSugar, name, assignment)
    {
        Type = type;
        ModifierSugar = modifierSugar;
        Name = name;
        Assignment = assignment;
    }
    
    public IEnumerable GetInstructionsRecursive()
    {
        yield return Type;
        yield return new CreateVariableInstruction(Name.Terminal
            , GetModifier(ModifierSugar), true, Location);
        
        if (Assignment != null)
        {
            // target
            yield return new PushNamedVariableInstruction(Name.Terminal, Location);
            // source
            yield return Assignment;
            // assign
            yield return new AssignVariableInstruction(Location);
        }
    }
}