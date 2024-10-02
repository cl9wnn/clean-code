namespace MarkdownLibrary;

public class Program
{
    static void Main(string[] args)
    {
        TokenParser tkn = new TokenParser();

        HtmlRenderer rndr = new HtmlRenderer();

        string input = "# Заголовок __с _разными_ символами__";
        var result1 = tkn.Process(input);
        Console.WriteLine(rndr.Render(input, result1));
    }
}

