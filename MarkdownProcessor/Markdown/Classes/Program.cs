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

        TokenParser tokenParser = new TokenParser(TagsDictionary);
        LineParser lineParser = new LineParser(tokenParser);

        HtmlRenderer rndr = new HtmlRenderer(TagsDictionary);

        string input = "# Заголовок __с _разными_ символами__\nКакой-то текст";
        string input2 = "\\# Вот это заголовок";

        var lines = lineParser.Parse(input2);
        Console.WriteLine(rndr.Render(lines));
    }
}

