namespace MarkdownLibrary;

public class SingleTagFactory
{
    private readonly Dictionary<string, TagElement> _singleTagDictionary;

    public SingleTagFactory()
    {
        _singleTagDictionary = new Dictionary<string, TagElement>
        {
             {"#", new HeaderTag()},
            { "*", new MarkedListTag() },
            { "+", new MarkedListTag() },
            { "-", new MarkedListTag() },
        };
    }

    public TagElement GetTag(string line)
    {
        var trimmedLine = line.TrimStart();

        if (trimmedLine.Length == 0)
        {
            return null;
        }

        var symbol = trimmedLine[0].ToString();

        if (_singleTagDictionary.TryGetValue(symbol, out var tag) && char.IsWhiteSpace(trimmedLine[1]))
        {
            return tag;
        }
        return null;
    }
}
