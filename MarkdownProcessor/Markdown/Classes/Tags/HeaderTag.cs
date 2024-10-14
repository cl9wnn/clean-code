
namespace MarkdownLibrary
{
    public class HeaderTag : TagElement
    {
        public override string[] MdTags => ["#"];
        public override string OpenHtmlTag => "<h1>";
        public override string CloseHtmlTag => "</h1>";
        public override int MdLength => 1;

        public string RenderHeaderLine(string line)
        {
            return $"{OpenHtmlTag}{line}{CloseHtmlTag}";
        }
    }
}
