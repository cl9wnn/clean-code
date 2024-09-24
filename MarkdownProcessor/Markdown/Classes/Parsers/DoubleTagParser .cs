namespace MarkdownLibrary;

public class DoubleTagParser : IFragmentParser
{
    private readonly TagElement _tag;

    //TODO: проверка на тип тега
    public DoubleTagParser(TagElement tag)
    {
        _tag = tag;
    }
    public string ParseLine(string markdownLine)
    {
        var content = markdownLine.Trim('_').Trim();

        return $"<{_tag.HtmlTag}>{content}</{_tag.HtmlTag}>";
    }
}