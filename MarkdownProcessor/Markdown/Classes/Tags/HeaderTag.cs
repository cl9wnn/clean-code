
namespace MarkdownLibrary
{
    public class HeaderTag : TagElement
    {
        public override string OpenHtmlTag => "<h1>";
        public override string CloseHtmlTag => "</h1>";
        public override string MdTag => "#";
        public override bool IsDoubleTag => false;
        public string RenderHeaderLine(string line)
        {
            return $"{OpenHtmlTag}{line}{CloseHtmlTag}";
        }
    }
}
