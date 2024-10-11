namespace MarkdownLibrary;

public class MarkedListTag : TagElement
{
    public override string MdTag => "*";
    public override string OpenHtmlTag => "<li>";
    public override string CloseHtmlTag => "</li>";
    public override bool IsDoubleTag => false;
    public string RenderMarkedListLine(string line)
    {
        return $"{OpenHtmlTag}{line}{CloseHtmlTag}";
    }

}
