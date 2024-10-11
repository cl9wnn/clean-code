using System.Net.Mail;

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
            var (content, lineType) = GetLineInfo(line); 
            var tokens = _tokenParser.Parse(content);
            lineTokens.Add(new Line(tokens, lineType)); 
        }

        return lineTokens;
    }

    //TODO: заменить символы на свойство из класса тега
    private (string content, TagElement tag) GetLineInfo(string line)
    {
        if (IsHeader(line))
        {
            var headerTag = new HeaderTag();
            return (line.TrimStart('#').Trim(), headerTag);
        }
        else if (IsMarkedList(line))
        {
            var listTag = new MarkedListTag();
            string content = line.TrimStart().TrimStart('*', '-', '+').Trim();
            return (content, listTag);
        }
        else
        {
            return (line.Trim(), null);
        }
    }


    //TODO: убрать # и взять из тега
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

    //TODO: Сделать поддержку всех символов через класс тегов нормально
    private bool IsMarkedList(string line)
    {
        if (!string.IsNullOrWhiteSpace(line))
        {
            string trimmedLine = line.TrimStart();

            if (trimmedLine.Length > 1 && "*+-".Any(c => trimmedLine.StartsWith(c)))
            {
                return char.IsWhiteSpace(trimmedLine[1]);
            }
        }
        return false;
    }
}
