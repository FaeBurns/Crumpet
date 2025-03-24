using System.Diagnostics.CodeAnalysis;
using Crumpet.Instructions;
using Crumpet.Instructions.Unary;
using Crumpet.Interpreter.Instructions;
using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;


using Parser;
using Parser.NodeConstraints;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Expressions;

public class PrimaryExpressionNode : NonTerminalNode, INonTerminalNodeFactory
{

    protected PrimaryExpressionNode(params IEnumerable<ASTNode?> children) : base(children)
    {
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<PrimaryExpressionNode>(new SequenceConstraint(
            new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER),
            new ZeroOrMoreConstraint(new NonTerminalConstraint<PrimaryExpressionNodeIdentifierVariant.ArgumentCollator>())),
            GetNodeConstructor<PrimaryExpressionNodeIdentifierVariant>());

        yield return new NonTerminalDefinition<PrimaryExpressionNodeIdentifierVariant.ArgumentCollator>(
            new SequenceConstraint(
                new OrConstraint(new CrumpetTerminalConstraint(CrumpetToken.PERIOD), new CrumpetTerminalConstraint(CrumpetToken.DEREFERENCE)),
                new CrumpetTerminalConstraint(CrumpetToken.IDENTIFIER)),
            GetNodeConstructor<PrimaryExpressionNodeIdentifierVariant.ArgumentCollator>());
        
        yield return new NonTerminalDefinition<PrimaryExpressionNode>(new NonTerminalConstraint<LiteralConstantNode>(), GetNodeConstructor<PrimaryExpressionNodeLiteralConstantVariant>());
        
        yield return new NonTerminalDefinition<PrimaryExpressionNode>(new SequenceConstraint(
            new CrumpetRawTerminalConstraint(CrumpetToken.LPARAN),
            new NonTerminalConstraint<ExpressionNode>(),
            new CrumpetRawTerminalConstraint(CrumpetToken.RPARAN)
            ),
            GetNodeConstructor<PrimaryExpressionNodeNestedExpressionVariant>());
    }
}

public class PrimaryExpressionNodeIdentifierVariant : PrimaryExpressionNode, IInstructionProvider
{
    public IdentifierNode FirstNode { get; }
    public IEnumerable<ArgumentCollator> Args { get; }

    public PrimaryExpressionNodeIdentifierVariant(IdentifierNode identifier, IEnumerable<ArgumentCollator> args) : base(identifier)
    {
        FirstNode = identifier;
        Args = args.ToArray();
        foreach (ArgumentCollator arg in Args)
        {
            ImplicitChildren.Add(arg.Sugar);
            ImplicitChildren.Add(arg.Identifier);
        }
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return new PushNamedVariableInstruction(FirstNode.Terminal, Location);
        
        foreach (ArgumentCollator arg in Args)
        {
            if (arg.Sugar.Token.TokenId == CrumpetToken.DEREFERENCE)
                yield return new DereferencePointerInstruction(Location);

            yield return new PopAndSearchFieldInstruction(arg.Identifier.Terminal, Location);
        }
    }
    
    public class ArgumentCollator : NonTerminalNode
    {
        public TerminalNode<CrumpetToken> Sugar { get; }
        public IdentifierNode Identifier { get; }

        public ArgumentCollator(TerminalNode<CrumpetToken> sugar, IdentifierNode identifier)
        {
            Sugar = sugar;
            Identifier = identifier;
        }
    }
}

public class PrimaryExpressionNodeLiteralConstantVariant : PrimaryExpressionNode, IInstructionProvider
{
    public LiteralConstantNode LiteralConstant { get; }

    public PrimaryExpressionNodeLiteralConstantVariant(LiteralConstantNode literalConstant) : base(literalConstant)
    {
        LiteralConstant = literalConstant;
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return LiteralConstant;
    }
}

public class PrimaryExpressionNodeNestedExpressionVariant : PrimaryExpressionNode, IInstructionProvider
{
    public ExpressionNode Expression { get; }

    public PrimaryExpressionNodeNestedExpressionVariant(ExpressionNode expression) : base(expression)
    {
        Expression = expression;
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return Expression;
    }
}
