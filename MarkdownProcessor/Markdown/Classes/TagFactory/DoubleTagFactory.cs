namespace MarkdownLibrary;

public class DoubleTagFactory : ITagFactory
{
    private readonly IEnumerable<TagElement> _tags;

    public DoubleTagFactory(IEnumerable<TagElement> tags)
    {
        _tags = tags;
    }

    public TagElement? GetTag(string symbol)
    {
        foreach (var tag in _tags)
        {
            if (tag.IsDoubleTag == true && tag.MdTags.Contains(symbol))
            {
                return tag;
            }
        }
        return null;
    }
}
