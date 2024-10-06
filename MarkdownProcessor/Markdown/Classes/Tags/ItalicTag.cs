using MarkdownLibrary;

public class ItalicTag : TagElement
{
    public override string OpenHtmlTag =>  "<em>";
    public override string CloseHtmlTag => "</em>";
    public override string MdTag => "_";
    public override bool IsDoubleTag => true;

}

