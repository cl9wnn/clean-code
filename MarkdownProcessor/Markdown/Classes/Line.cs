namespace MarkdownLibrary;

public class Line
{
    public IEnumerable<Token> Tokens { get; set; }
    public TagElement Type { get; set; } 

    public Line(IEnumerable<Token> tokens, TagElement type)
    {
        Tokens = tokens;
        Type = type;
    }
}

public enum LineType
{
    Header,
    List,
    Text
}