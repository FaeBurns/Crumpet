namespace Crumpet.Parser.Elements;

public class MultipleParserElements : ParserElement
{
    public IEnumerable<ParserElement> Collection { get; }
    public bool TransformToSeparateArguments { get; }

    public MultipleParserElements(IEnumerable<ParserElement> collection, bool transformToSeparateArguments)
    {
        Collection = collection;
        TransformToSeparateArguments = transformToSeparateArguments;
    }

    public override IEnumerable<object> TransformForConstructor()
    {
        // select many so they accumulate into the one enumerable
        object[] transformedElements = Collection.SelectMany(e => e.TransformForConstructor()).ToArray();

        // if each element should be a separate argument
        // then return them as such
        if (TransformToSeparateArguments)
            return transformedElements;

        // otherwise simply return
        return [transformedElements];
    }
}