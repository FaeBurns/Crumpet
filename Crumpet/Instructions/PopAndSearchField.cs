using Crumpet.Exceptions;
using Crumpet.Interpreter;
using Crumpet.Interpreter.Functions;
using Crumpet.Interpreter.Instructions;
using Crumpet.Interpreter.Variables;
using Shared;

namespace Crumpet.Instructions;

public class PopAndSearchField : Instruction
{
    private readonly string[] m_path;

    public PopAndSearchField(IEnumerable<string> path, SourceLocation location) : base(location)
    {
        m_path = path.ToArray();
    }
    
    public override void Execute(InterpreterExecutionContext context)
    {
        Variable target = context.VariableStack.Pop();
        ValueSearchResult valueSearchResult = context.ValueSearcher.Find(target, m_path);

        if (valueSearchResult.Result is null)
            throw new InterpreterException(
                context,
                ExceptionConstants.VALUE_SEARCH_FAILED.Format(
                    String.Join('.', new ReadOnlySpan<string>(m_path, 0, valueSearchResult.DepthReached)!),
                    String.Join('.', m_path)));
        
        context.VariableStack.Push(valueSearchResult.Result);
    }
}