namespace Crumpet.Parser;

[System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class TokenConverterAttribute<TToken, TResultNode> : Attribute
{
    public TToken InputTokenType { get; }
    public Type ResultNodeType { get; }
    
    public TokenConverterAttribute(TToken inputTokenType)
    {
        InputTokenType = inputTokenType;

        ResultNodeType = typeof(TResultNode);
    }
}