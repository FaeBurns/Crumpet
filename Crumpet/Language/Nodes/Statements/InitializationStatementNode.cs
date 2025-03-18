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

public class InitializationStatementNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public TypeNode Type { get; }
    public TerminalNode<CrumpetToken>? ModifierSugar { get; }
    public IdentifierNode Name { get; }

    public VariableModifier VariableModifier
    {
        get
        {
            if (ModifierSugar is null)
                return VariableModifier.COPY;

            // re-use multiply as multiple identical tokens cannot exist
            if (ModifierSugar.Token.TokenId == CrumpetToken.REFERENCE)
                return VariableModifier.POINTER;

            throw new UnreachableException();
        }
    }

    public InitializationStatementNode(TypeNode type, TerminalNode<CrumpetToken>? modifierSugar, IdentifierNode name) : base(type, name)
    {
        Type = type;
        ModifierSugar = modifierSugar;
        Name = name;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<InitializationStatementNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.REFERENCE)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)),
            GetNodeConstructor<InitializationStatementNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return new CreateVariableInstruction(Name.Terminal, Type.FullName, VariableModifier);
    }
}