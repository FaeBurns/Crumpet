using Crumpet.Instructions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables.Types;
using Lexer;
using Parser;
using Parser.Nodes;

namespace Crumpet.Language.Nodes.Terminals;

public class StringLiteralNode : TerminalNode<CrumpetToken>, ITerminalNodeFactory<CrumpetToken>, IInstructionProvider
{
    public string StringLiteral { get; }
    public StringLiteralNode(Token<CrumpetToken> token) : base(token)
    {
        StringLiteral = token.Value;
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        // \".*\"
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.STRING, GetNodeConstructor<StringLiteralNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return new PushConstantInstruction(new BuiltinTypeInfo<string>(), StringLiteral);
    }
}