namespace MarkdownLibrary;

public class Line
{
    public IEnumerable<Token> Tokens { get; set; }
    public TagElement Type { get; set; } 
    public int IndentLevel { get; }

    public Line(IEnumerable<Token> tokens, TagElement type, int indentLevel)
    {
        Tokens = tokens;
        Type = type;
        IndentLevel = indentLevel;
    }
}

public enum LineType
{
    Header,
    List,
    Text
}