using System.Diagnostics;
using Crumpet.Interpreter.Interpreter.Variables;
using Crumpet.Interpreter.Interpreter.Variables.InstanceValues;

namespace Crumpet.Interpreter.Interpreter;

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
        InstanceReference? instance = m_scope.FindReference(segments[0]);

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

    private ValueSearchResult FindObjectFieldRecursive(InstanceReference searchTarget, string[] segments, int depth)
    {
        InstanceReference? nextTarget = FindObjectField(searchTarget, segments[0]);
        
        if (segments.Length == 1)
        {
            return new ValueSearchResult(nextTarget, depth + 1);
        }
        
        if (nextTarget is null)
            return new ValueSearchResult(null, depth);
        
        return FindObjectFieldRecursive(nextTarget, segments.Skip(1).ToArray(), depth + 1);
    }

    private InstanceReference? FindObjectField(InstanceReference searchTarget, string name)
    {
        if (searchTarget.Value is ObjectInstance objectInstance && objectInstance.Fields.Has(name))
        {
            return objectInstance.Fields[name];
        }

        return null;
    }

}

public class ValueSearchResult(InstanceReference? result, int depthReached)
{
    public InstanceReference? Result { get; } = result;
    public int DepthReached { get; } = depthReached;
}
