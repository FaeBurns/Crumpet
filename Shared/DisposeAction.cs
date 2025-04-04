namespace Shared;

public class DisposeAction : IDisposable
{
    private readonly Action m_action;

    public DisposeAction(Action action)
    {
        m_action = action;
    }
    
    public void Dispose()
    {
        m_action.Invoke();
    }
}