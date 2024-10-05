using System.Text;

namespace MarkdownLibrary;

public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly TokenParser _tokenParser;
    private readonly IRenderer _renderer;

    //TODO: Вынести в нужную сущность

    private readonly Dictionary<string, TagElement> TagsDictionary = new()
        {
            { "_", new ItalicTag() },
            { "__", new BoldTag() },
            { "#", new HeaderTag() }
        };

    public MarkdownProcessor()
    {
        _tokenParser = new TokenParser(TagsDictionary);
        _renderer = new HtmlRenderer(TagsDictionary);
    }
    public string ConvertToHtml(string markdownText)
    {
        var result = _tokenParser.Process(markdownText);
        return _renderer.Render(result);
    }
}
    

