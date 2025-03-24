using Crumpet.Instructions;
using Crumpet.Instructions.Flow;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Statements;
using Crumpet.Language.Nodes.Terminals;



using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class FunctionDeclarationNode : NonTerminalNode, INonTerminalNodeFactory, IInstructionProvider
{
    public TypeNode ReturnType { get; }
    public IdentifierNode Name { get; }
    public ParameterListNode Parameters { get; }
    public StatementBodyNode StatementBody { get; }
    public VariableModifier ReturnModifier { get; }

    public FunctionDeclarationNode(TypeNode returnType, TerminalNode<CrumpetToken>? pointerSugar, IdentifierNode name, ParameterListNode? parameters, StatementBodyNode statementBody) : base(returnType, pointerSugar!, name, parameters!, statementBody)
    {
        ReturnType = returnType;
        Name = name;
        StatementBody = statementBody;

        // don't know why this thinks it'll never be null when it's cleary annotated to be
        Parameters = parameters ?? new ParameterListNode();
        ReturnModifier = pointerSugar == null ? VariableModifier.COPY : VariableModifier.POINTER;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<FunctionDeclarationNode>(
            new SequenceConstraint(
                new CrumpetRawTerminalConstraint(CrumpetToken.KW_FUNC),
                new NonTerminalConstraint<TypeNode>(),
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
                new OptionalConstraint(new NonTerminalConstraint<ParameterListNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN),
                new NonTerminalConstraint<StatementBodyNode>()),
            GetNodeConstructor<FunctionDeclarationNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return StatementBody;

        // implicit return. This will be skipped if the statement includes a return 
        yield return new ReturnInstruction(false, Location);
        
        // target for returns to hit
        yield return new ReturnLabelInstruction(Location);
        
        if (ReturnType.FullName != "void")
            yield return new AssertReturnTypeInstruction(ReturnType.FullName, ReturnModifier, Location);
    }
}