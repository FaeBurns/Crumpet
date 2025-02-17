namespace Crumpet.Interpreter.Parser.Nodes;

public interface INonTerminalNodeFactory
{
     static abstract IEnumerable<NonTerminalDefinition> GetNonTerminals();
}

public interface ITerminalNodeFactory<T> where T : Enum
{
     static abstract IEnumerable<TerminalDefinition<T>> GetTerminals();
}