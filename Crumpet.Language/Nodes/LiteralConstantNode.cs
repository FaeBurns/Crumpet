using Crumpet.Language.Nodes.Constraints;
using Crumpet.Language.Nodes.Terminals;
using Crumpet.Parser;
using Crumpet.Parser.Nodes;

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
        yield return new NonTerminalDefinition<LiteralConstantNode>(new CrumpetTerminalConstraint(CrumpetToken.STRING), GetNodeConstructor<LiteralConstantNodeStringVariant>());
        yield return new NonTerminalDefinition<LiteralConstantNode>(new CrumpetTerminalConstraint(CrumpetToken.INT), GetNodeConstructor<LiteralConstantNodeIntVariant>());
        yield return new NonTerminalDefinition<LiteralConstantNode>(new CrumpetTerminalConstraint(CrumpetToken.FLOAT), GetNodeConstructor<LiteralConstantNodeFloatVariant>());
        yield return new NonTerminalDefinition<LiteralConstantNode>(new CrumpetTerminalConstraint(CrumpetToken.BOOL), GetNodeConstructor<LiteralConstantNodeBoolVariant>());
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