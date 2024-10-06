namespace MarkdownLibrary
{
    public interface IRenderer
    {
        public string Render(IEnumerable<Line> tokens);
    }
}
