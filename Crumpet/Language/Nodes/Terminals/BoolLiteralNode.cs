using Crumpet.Instructions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
using Lexer;
using Parser;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class BoolLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>, IInstructionProvider
{
    public bool BoolLiteral { get; }

    public BoolLiteralNode(Token<CrumpetToken> token) : base(token)
    {
        BoolLiteral = Convert.ToBoolean(token.Value);
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.BOOL, GetNodeConstructor<BoolLiteralNode>());
    }

    public IEnumerable<Instruction> GetInstructions()
    {
        yield return new PushConstantInstruction(new BuiltinTypeInfo<bool>(), BoolLiteral);
    }
}