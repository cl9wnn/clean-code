﻿namespace MarkdownLibrary;

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

        string input = "_одинарное __двойное__ не_ работает.";

        var lines = lineParser.Parse(input);
        Console.WriteLine(rndr.Render(lines));
    }
}

