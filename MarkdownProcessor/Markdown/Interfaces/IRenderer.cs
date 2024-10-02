namespace MarkdownLibrary
{
    public interface IRenderer
    {
        public string Render(string text, IEnumerable<Token> tokens);
    }
}
