using Crumpet.Interpreter.Parser.Elements;
using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser.NodeConstraints;

public abstract class NodeConstraint
{
    public abstract override string ToString();

    public abstract ParserElement? WalkStream<T>(ObjectStream<TerminalNode<T>> stream, ASTNodeRegistry<T> registry) where T : Enum;
}