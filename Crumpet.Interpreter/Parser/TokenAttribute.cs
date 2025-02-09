namespace Crumpet.Interpreter.Parser;

[AttributeUsage(AttributeTargets.Field)]
internal sealed class TokenAttribute : Attribute
{
    public string Regex { get; }

    public bool IsNewline { get; set; }
    public bool IsComment { get; set; }

    public TokenAttribute(string regex)
    {
        Regex = regex;
    }
}