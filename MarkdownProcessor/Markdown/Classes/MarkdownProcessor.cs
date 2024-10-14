using System.Net.Http.Headers;
using System.Text;

namespace MarkdownLibrary;

public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly IParser<Token> _tokenParser;
    private readonly IParser<Line> _lineParser;
    private readonly IRenderer _renderer;
    private readonly IFileParser _fileParser;
    private readonly SingleTagFactory _tagFactory;

    private readonly Dictionary<string, TagElement> _doubleTagDictionary = new()
    {
            { "_", new ItalicTag()},
            { "__", new BoldTag() },
    };

    private readonly Dictionary<string, TagElement> _singleTagDictionary = new()
    {
            { "#", new HeaderTag()},
            { "*", new MarkedListTag() },
            { "+", new MarkedListTag() },
            { "-", new MarkedListTag() },

    };

    public MarkdownProcessor()
    {
        _tagFactory = new SingleTagFactory();
        _tokenParser = new TokenParser(_doubleTagDictionary);
        _lineParser = new LineParser(_tokenParser, _tagFactory);
        _fileParser  = new MdFileParser();
        _renderer = new HtmlRenderer(_doubleTagDictionary, _singleTagDictionary);
    }

    public string ConvertToHtmlFromString(string markdownText)
    {
        var lines = _lineParser.Parse(markdownText);

        return _renderer.Render(lines);
    }

    public string ConvertToHtmlFromFile(string filePath)
    {
        var parsedFile = _fileParser.Parse(filePath);
        var lines = _lineParser.Parse(parsedFile);

        return _renderer.Render(lines);
    }


}
    

