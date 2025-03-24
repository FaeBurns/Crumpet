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
        // get everything except the quotes on the outside and replace all \" with "
        StringLiteral = token.Value[new Range(Index.FromStart(1), Index.FromEnd(1))].Replace("\\\"", "\"");
    }

    public static IEnumerable<TerminalDefinition<CrumpetToken>> GetTerminals()
    {
        // \".*\"
        yield return new TerminalDefinition<CrumpetToken>(CrumpetToken.STRING, GetNodeConstructor<StringLiteralNode>());
    }

    public IEnumerable GetInstructionsRecursive()
    {
        yield return new PushConstantInstruction(BuiltinTypeInfo.String, StringLiteral, VariableModifier.COPY, Location);
    }
}