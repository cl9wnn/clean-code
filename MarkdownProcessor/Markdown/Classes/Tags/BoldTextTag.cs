using MarkdownLibrary;

public class BoldTextTag : TagElement
{
    public override string MdTag => "__";
    public override string HtmlTag => "strong";
}
