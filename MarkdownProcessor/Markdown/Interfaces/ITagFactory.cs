namespace MarkdownLibrary;

public interface ITagFactory
{
    TagElement? GetTag(string token);
}
