namespace Crumpet.Interpreter.Parser.Nodes;

public interface INonTerminalNodeFactory
{
     static abstract IEnumerable<NonTerminalDefinition> GetNonTerminals();
}

public interface ITerminalNodeFactory
{
     static abstract IEnumerable<TerminalDefinition> GetTerminals();
}