using Crumpet.Interpreter.Parser.Nodes;

namespace Crumpet.Interpreter.Parser;

public interface INodeFactoryCollection
{
    public IEnumerable<Type> GetNonTerminalFactories();
    public IEnumerable<Type> GetTerminalFactories();
}