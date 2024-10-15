namespace MarkdownLibrary;

public class LineParser : IParser<Line>
{
    private readonly IParser<Token> _tokenParser;
    private readonly ITagFactory _tagFactory;
    public LineParser(IParser<Token> tokenParser, ITagFactory tagFactory)
    {
        _tokenParser = tokenParser;
        _tagFactory = tagFactory;
    }

    public IEnumerable<Line> Parse(string markdownText)
    {
        var lines = markdownText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        var lineTokens = new List<Line>();

        foreach (var line in lines)
        {
            var indentLevel = GetIndentLevel(line);
            var tag = _tagFactory.GetTag(line);

            var content = tag != null ? TrimLine(line, tag) : line;

            var tokens = _tokenParser.Parse(content);

            lineTokens.Add(new Line(tokens, (ILineTag)tag, indentLevel));
        }

        return lineTokens;
    }

    public int GetIndentLevel(string input)
    {
        int indentSize = 2;
        int spaceCount = input.TakeWhile(char.IsWhiteSpace).Count();

        return spaceCount / indentSize;
    }

  
    public string TrimLine(string line, TagElement tag)
    {
        string trimmedLine = line.TrimStart();

        foreach (var mdTag in tag.MdTags)
        {
            if (trimmedLine.StartsWith(mdTag))
            {
                return trimmedLine.TrimStart().Substring(mdTag.Length + 1);
            }
        }

        return line;
    }
}
