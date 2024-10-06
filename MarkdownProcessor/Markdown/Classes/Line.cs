namespace MarkdownLibrary;

public class Line
{
    public IEnumerable<Token> Tokens { get; set; }
    public bool IsHeader { get; set; }

    public Line(IEnumerable<Token> tokens, bool isHeader)
    {
        Tokens = tokens;
        IsHeader = isHeader;
    }
}