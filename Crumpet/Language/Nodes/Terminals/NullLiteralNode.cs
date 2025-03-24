using Crumpet.Instructions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
using Lexer;
using Parser;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class NullLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>, IInstructionProvider
{
    public NullLiteralNode(Token<CrumpetToken> token) : base(token)
    {
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.NULL, GetNodeConstructor<NullLiteralNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return new PushConstantInstruction(new NullTypeInfo(), null!, VariableModifier.POINTER, Location);
    }
}