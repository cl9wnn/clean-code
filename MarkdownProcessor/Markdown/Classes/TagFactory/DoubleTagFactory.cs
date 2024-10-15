namespace MarkdownLibrary;

public class DoubleTagFactory: ITagFactory
{
    private readonly Dictionary<string, TagElement> _tagDictionary;

    public DoubleTagFactory()
    {
        _tagDictionary = new()
    {
            { "_", new ItalicTag()},
            { "__", new BoldTag()},
    };
}

    public TagElement? GetTag(string symbol)
    {
        return _tagDictionary.TryGetValue(symbol, out var tag) ? tag : null;
    }

}
