namespace MarkdownLibrary;

public class MarkedListTag : TagElement
{
    public override string[] MdTags => ["*", "-", "+"];
    public override string OpenHtmlTag => "<li>";
    public override string CloseHtmlTag => "</li>";
    public override int MdLength => 1;

    public string RenderMarkedListLine(string line)
    {
        return $"{OpenHtmlTag}{line}{CloseHtmlTag}";
    }

}
