namespace MarkdownLibrary;

public class Program
{
    public static void Main(string[] args)
    {
        Dictionary<string, TagElement> TagsDictionary = new()
        {
            { "_", new ItalicTag() },
            { "__", new BoldTag() },
            { "#", new HeaderTag() }
        };

        TokenParser tkn = new TokenParser(TagsDictionary);

        HtmlRenderer rndr = new HtmlRenderer(TagsDictionary);

        string input = @"# Заголовок __с _разными_ символами__";
        var result1 = tkn.Process(input);
        Console.WriteLine(rndr.Render(result1));
    }
}

