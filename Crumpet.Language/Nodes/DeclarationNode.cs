using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Language.Nodes;

public class DeclarationNode : NonTerminalNode, INonTerminalNodeFactory
{
    public ASTNode Variant { get; }

    protected DeclarationNode(ASTNode variant) : base(variant)
    {
        Variant = variant;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition<DeclarationNode>(new NonTerminalConstraint<FunctionDeclarationNode>(), GetNodeConstructor<DeclarationNodeFunctionVariant>());
        yield return new NonTerminalDefinition<DeclarationNode>(new NonTerminalConstraint<TypeDeclarationNode>(), GetNodeConstructor<DeclarationNodeTypeVariant>());
    }
}

public class DeclarationNodeFunctionVariant : DeclarationNode
{
    public FunctionDeclarationNode FunctionDeclaration { get; }

    public DeclarationNodeFunctionVariant(FunctionDeclarationNode functionDeclaration) : base(functionDeclaration)
    {
        FunctionDeclaration = functionDeclaration;
    }
}

public class DeclarationNodeTypeVariant : DeclarationNode
{
    public TypeDeclarationNode TypeDeclaration { get; }

    public DeclarationNodeTypeVariant(TypeDeclarationNode typeDeclaration) : base(typeDeclaration)
    {
        TypeDeclaration = typeDeclaration;
    }
}