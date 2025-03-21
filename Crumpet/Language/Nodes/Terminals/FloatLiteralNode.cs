using Crumpet.Instructions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
using Lexer;
using Parser;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class FloatLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>, IInstructionProvider
{
    public float FloatLiteral { get; }

    public FloatLiteralNode(Token<CrumpetToken> token) : base(token)
    {
        FloatLiteral = Convert.ToSingle(token.Value);
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.FLOAT, GetNodeConstructor<FloatLiteralNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return new PushConstantInstruction(BuiltinTypeInfo.Float, FloatLiteral, Location);
    }
}