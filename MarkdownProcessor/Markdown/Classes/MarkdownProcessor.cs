using System.Net.Http.Headers;
using System.Text;

namespace MarkdownLibrary;

public class MarkdownProcessor : IMarkdownProcessor
{
    private readonly IParser<Token> _tokenParser;
    private readonly IParser<Line> _lineParser;
    private readonly IRenderer _renderer;
    private readonly IFileParser _fileParser;

    public IEnumerable<string> tags = ["_", "__", "*", "#", "-", "+"];

    public MarkdownProcessor()
    {
        _tokenParser = new TokenParser(new DoubleTagFactory());
        _lineParser = new LineParser(_tokenParser, new SingleTagFactory());
        _fileParser  = new MdFileParser();
        _renderer = new HtmlRenderer(tags, new LineRenderer(), new ListRenderer());
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
    

