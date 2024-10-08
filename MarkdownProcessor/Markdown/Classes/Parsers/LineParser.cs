namespace MarkdownLibrary;

public class LineParser : IParser<Line>
{
    private readonly TokenParser _tokenParser;

    public LineParser(TokenParser tokenParser)
    {
        _tokenParser = tokenParser;
    }

    public IEnumerable<Line> Parse(string markdownText)
    {
        var lines = markdownText.Split('\n');
        var lineTokens = new List<Line>();

        foreach (var line in lines)
        {
            bool isHeader = IsHeader(line);
            var content = isHeader ? line.TrimStart('#').Trim() : line;

            var tokens = _tokenParser.Parse(content);

            lineTokens.Add(new Line(tokens, isHeader));
        }

        return lineTokens;
    }

    private bool IsHeader(string line)
    {
        if (!string.IsNullOrEmpty(line) && line.TrimStart().StartsWith("#"))
        {
            int headerLevel = line.TakeWhile(c => c == '#').Count();

            if (line.Length > headerLevel && line[headerLevel] == ' ')
            {
                return true;
            }
        }
        return false;
    }

}
