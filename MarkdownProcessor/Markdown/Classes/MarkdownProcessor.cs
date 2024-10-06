using System.Net.Http.Headers;
using System.Text;

namespace MarkdownLibrary;

public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly TokenParser _tokenParser;
    private readonly LineParser _lineParser;
    private readonly IRenderer _renderer;

    //TODO: Вынести в нужную сущность

    private readonly Dictionary<string, TagElement> TagsDictionary = new()
    {
            { "_", new ItalicTag()},
            { "__", new BoldTag() },
    };

    public MarkdownProcessor()
    {
        _tokenParser = new TokenParser(TagsDictionary);
        _lineParser = new LineParser(_tokenParser);
        _renderer = new HtmlRenderer(TagsDictionary);
    }
    public string ConvertToHtml(string markdownText)
    {
        var lines = _lineParser.Parse(markdownText);

        return _renderer.Render(lines);
    }
}
    

