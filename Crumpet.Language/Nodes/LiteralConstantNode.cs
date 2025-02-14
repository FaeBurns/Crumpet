using Crumpet.Interpreter.Parser;
using Crumpet.Interpreter.Parser.NodeConstraints;
using Crumpet.Interpreter.Parser.Nodes;
using Crumpet.Language.Nodes.Terminals;

namespace Crumpet.Language.Nodes;

public abstract class LiteralConstantNode : NonTerminalNode, INonTerminalNodeFactory
{
    private readonly ASTNode m_childRef;

    protected LiteralConstantNode(ASTNode childRef) : base(childRef)
    {
        m_childRef = childRef;
    }

    public static IEnumerable<NonTerminalDefinition> GetNonTerminals()
    {
        yield return new NonTerminalDefinition("literalConstant", new NamedTerminalConstraint("stringLiteral"), GetNodeConstructor<LiteralConstantNodeStringVariant>());
        yield return new NonTerminalDefinition("literalConstant", new NamedTerminalConstraint("intLiteral"), GetNodeConstructor<LiteralConstantNodeIntVariant>());
        yield return new NonTerminalDefinition("literalConstant", new NamedTerminalConstraint("floatLiteral"), GetNodeConstructor<LiteralConstantNodeFloatVariant>());
        yield return new NonTerminalDefinition("literalConstant", new NamedTerminalConstraint("boolLiteral"), GetNodeConstructor<LiteralConstantNodeBoolVariant>());
    }
}

public class LiteralConstantNodeStringVariant : LiteralConstantNode
{
    public StringLiteralNode StringLiteral { get; }

    public LiteralConstantNodeStringVariant(StringLiteralNode stringLiteral) : base(stringLiteral)
    {
        StringLiteral = stringLiteral;
    }
}

public class LiteralConstantNodeIntVariant : LiteralConstantNode
{
    public IntLiteralNode IntLiteral { get; }
    
    public LiteralConstantNodeIntVariant(IntLiteralNode intLiteral) : base(intLiteral)
    {
        IntLiteral = intLiteral;
    }
}

public class LiteralConstantNodeFloatVariant : LiteralConstantNode
{
    public FloatLiteralNode FloatLiteral { get; }

    public LiteralConstantNodeFloatVariant(FloatLiteralNode floatLiteral) : base(floatLiteral)
    {
        FloatLiteral = floatLiteral;
    }
}

public class LiteralConstantNodeBoolVariant : LiteralConstantNode
{
    public BoolLiteralNode BoolLiteral { get; }

    public LiteralConstantNodeBoolVariant(BoolLiteralNode boolLiteral) : base(boolLiteral)
    {
        BoolLiteral = boolLiteral;
    }
}