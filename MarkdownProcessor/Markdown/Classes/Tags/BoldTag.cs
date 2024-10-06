using MarkdownLibrary;

public class BoldTag : TagElement
{
    public override string OpenHtmlTag => "<strong>";
    public override string CloseHtmlTag => "</strong>";
    public override string MdTag => "__";
    public override bool IsDoubleTag => true;

}
