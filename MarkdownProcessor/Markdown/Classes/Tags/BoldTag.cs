namespace MarkdownLibrary;

public class BoldTag : TagElement
{
    public override string[] MdTags => ["__", "**"];
    public override string OpenHtmlTag => "<strong>";
    public override string CloseHtmlTag => "</strong>";
    public override int MdLength => 2;
    public override bool IsDoubleTag => true;
}
