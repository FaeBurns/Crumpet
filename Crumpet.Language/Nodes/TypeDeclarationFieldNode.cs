﻿using System.Diagnostics;
using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes;

public class TypeDeclarationFieldNode : NonTerminalNode, INonTerminalNodeFactory
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

            if (ModifierSugar.Token.TokenId == CrumpetToken.REFERENCE)
                return VariableModifier.REFERENCE;

            // re-use multiply as multiple identical tokens cannot exist
            if (ModifierSugar.Token.TokenId == CrumpetToken.MULTIPLY)
                return VariableModifier.POINTER;

            throw new UnreachableException();
        }
    }

    public TypeDeclarationFieldNode(TypeNode type, TerminalNode<CrumpetToken>? modifierSugar, IdentifierNode name) : base(type, modifierSugar, name)
    {
        Type = type;
        ModifierSugar = modifierSugar;
        Name = name;
    }
    
    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<TypeDeclarationFieldNode>(
            new SequenceConstraint(
                new NonTerminalConstraint<TypeNode>(),
                new OptionalConstraint(new OrConstraint(new CrumpetTerminalConstraint(CrumpetToken.REFERENCE), new CrumpetTerminalConstraint(CrumpetToken.MULTIPLY))),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER), 
                new CrumpetRawTerminalConstraint(CrumpetToken.SEMICOLON)),
            GetNodeConstructor<TypeDeclarationFieldNode>());
    }

    public override string ToString()
    {
        return $"{Name}: {Type}";
    }
}