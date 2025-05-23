﻿using Crumpet.Instructions;
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
    public GenericTypeDeclarationListNode TypeParams { get; }
    public ParameterListNode Parameters { get; }
    public StatementBodyNode StatementBody { get; }
    public VariableModifier ReturnModifier { get; }

    public FunctionDeclarationNode(TypeNode returnType, TerminalNode<CrumpetToken>? pointerSugar, IdentifierNode name, GenericTypeDeclarationListNode typeParams, ParameterListNode? parameters, StatementBodyNode statementBody) : base(returnType, pointerSugar!, name, typeParams, parameters!, statementBody)
    {
        ReturnType = returnType;
        Name = name;
        TypeParams = typeParams;
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
                new NonTerminalConstraint<TypeNode>(), // return type
                new OptionalConstraint(new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY)), // return type variable modifier
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
                new NonTerminalConstraint<GenericTypeDeclarationListNode>(), // generic type args
                new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN), // parameters
                new OptionalConstraint(new NonTerminalConstraint<ParameterListNode>()),
                new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN),
                new NonTerminalConstraint<StatementBodyNode>()), // body
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
        {
            yield return ReturnType;
            yield return new AssertReturnTypeInstruction(ReturnModifier, Location);
        }
    }
}