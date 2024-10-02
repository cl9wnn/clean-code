using System.Text;

namespace MarkdownLibrary;

public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly TokenParser _tokenParser;
    private readonly IRenderer _renderer;

    public MarkdownProcessor()
    {
        _tokenParser = new TokenParser();
        _renderer = new HtmlRenderer();
    }
    public string ConvertToHtml(string markdownText)
    {
        var result = _tokenParser.Process(markdownText);
        return _renderer.Render(markdownText, result);
    }
}
    

