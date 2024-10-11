using System.Reflection.Metadata;

namespace MarkdownLibrary;

public class Program
{
    public static void Main(string[] args)
    {
        Dictionary<string, TagElement> TagsDictionary = new()
        {
            { "_", new ItalicTag() },
            { "__", new BoldTag() },
            {"#", new HeaderTag()}
        };

        TokenParser tokenParser = new TokenParser(TagsDictionary);
        LineParser lineParser = new LineParser(tokenParser);

        HtmlRenderer rndr = new HtmlRenderer(TagsDictionary);

        string input = "+ Пункт A\n+ Пункт B\n  + Подпункт 1\n  + Подпункт 2\n+ Пункт C";
        var lines = lineParser.Parse(input);
        Console.WriteLine(rndr.Render(lines));
    }
}

