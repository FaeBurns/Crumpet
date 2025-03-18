using Crumpet.Instructions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
using Lexer;
using Parser;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class IntLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>, IInstructionProvider
{
    public int IntLiteral { get; }

    public IntLiteralNode(Token<CrumpetToken> token) : base(token)
    {
        IntLiteral = Convert.ToInt32(token.Value);
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.INT, GetNodeConstructor<IntLiteralNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return new PushConstantInstruction(new BuiltinTypeInfo<int>(), IntLiteral);
    }
}