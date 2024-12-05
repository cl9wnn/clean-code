namespace MarkdownLibrary;

public class ParagraphTag : TagElement, ILineTag
{
    public override string[] MdTags => [""];
    public override string OpenHtmlTag => "<p>";
    public override string CloseHtmlTag => "</p>";
    public override int MdLength => 1;

    public string RenderLine(string line, int indentLevel)
    {
        return $"{OpenHtmlTag}{line}{CloseHtmlTag}";
    }
}