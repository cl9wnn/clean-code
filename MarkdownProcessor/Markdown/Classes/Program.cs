namespace MarkdownLibrary;

public class Program
{ 
    public static void Main(string[] args)
    {
        Dictionary<string, TagElement> TagsDictionary = new()
        {
            { "_", new ItalicTag() },
            { "__", new BoldTag() },
        };

        var tokenParser = new TokenParser(TagsDictionary);
        var lineParser = new LineParser(tokenParser);
        var rndr = new HtmlRenderer(TagsDictionary);
        var fileParser = new MdFileParser();
        var proc = new MarkdownProcessor();

    }
}
