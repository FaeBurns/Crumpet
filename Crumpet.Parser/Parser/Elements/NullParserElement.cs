namespace Crumpet.Interpreter.Parser.Elements;

/// <summary>
/// A value that can be returned that means null - used in optional returns.
/// </summary>
public class NullParserElement : ParserElement
{
    public override IEnumerable<object> TransformForConstructor()
    {
        return [null!];
    }
}