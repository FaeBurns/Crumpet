using Crumpet.Interpreter.Variables;

namespace Crumpet.Interpreter;

public class ValueSearcher
{
    private readonly Scope m_scope;

    public ValueSearcher(Scope scope)
    {
        m_scope = scope;
    }

    /// <summary>
    /// Finds a variable by its segments.
    /// </summary>
    /// <param name="identifier">The string that contains the full name of the target value</param>
    /// <param name="separator">The character separator to apply on <paramref name="identifier"/></param>
    /// <returns>The resulting reference or null if none was found</returns>
    public ValueSearchResult Find(string identifier, char separator = '.')
    {
        return Find(identifier.Split(separator));
    }

    /// <summary>
    /// Finds a variable by its segments.
    /// </summary>
    /// <param name="segments">The array of strings that make up the variable reference.</param>
    /// <returns>The resulting reference or null if none was found</returns>
    public ValueSearchResult Find(string[] segments)
    {
        // should not be possible - under what situation would this be called with no segments???
        if (segments.Length == 0)
            throw new ArgumentException();

        // get the root instance
        Variable? instance = m_scope.FindVariable(segments[0]);

        // if it wasn't found, return 0 and null output
        if (instance is null)
        {
            return new ValueSearchResult(null, 0);
        }

        // return if this was the only segment
        if (segments.Length == 1)
        {
            return new ValueSearchResult(instance, 1);
        }

        // otherwise do recursive check
        return FindObjectFieldRecursive(instance, segments.Skip(1).ToArray(), 1);
    }

    private ValueSearchResult FindObjectFieldRecursive(Variable searchTarget, string[] segments, int depth)
    {
        Variable? nextTarget = FindObjectField(searchTarget, segments[0]);

        if (segments.Length == 1)
        {
            return new ValueSearchResult(nextTarget, depth + 1);
        }

        if (nextTarget is null)
            return new ValueSearchResult(null, depth);

        return FindObjectFieldRecursive(nextTarget, segments.Skip(1).ToArray(), depth + 1);
    }

    private Variable? FindObjectField(Variable searchTarget, string name)
    {
        if (searchTarget.Value is UserObjectInstance objectInstance && objectInstance.Fields.Has(name))
        {
            return objectInstance.Fields[name];
        }

        return null;
    }

}

public class ValueSearchResult(Variable? result, int depthReached)
{
    public Variable? Result { get; } = result;
    public int DepthReached { get; } = depthReached;
}
