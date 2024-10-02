namespace MarkdownLibrary
{
    public class Token
    {
        public TagElement Tag { get; set; }
        public bool IsClosing { get; set; }
        public int StartIndex { get; set; }
        public Token(TagElement tag, int startIndex, bool isClosing = false)
        {
            Tag = tag;
            StartIndex = startIndex;
            IsClosing = isClosing;
        }

    }
}
