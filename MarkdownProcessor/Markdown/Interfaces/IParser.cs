namespace MarkdownLibrary;

public interface IParser<T>
{
    IEnumerable<T> Parse(string input);
}
