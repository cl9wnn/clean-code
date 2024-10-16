namespace MarkdownLibrary
{
    public class HeaderTag : TagElement, ILineTag
    {
        public override string[] MdTags => ["#"];
        public override string OpenHtmlTag => "<h1>";
        public override string CloseHtmlTag => "</h1>";
        public override int MdLength => 1;
        public override bool IsDoubleTag => false;

        public string RenderLine(string line, int indentLevel)
        {
            return $"{OpenHtmlTag}{line}{CloseHtmlTag}";
        }

    }
}
