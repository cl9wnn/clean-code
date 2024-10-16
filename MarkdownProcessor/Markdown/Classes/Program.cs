namespace MarkdownLibrary;

public class Program
{ 
    public static void Main(string[] args)
    {
        string input = "example_ text_";

        IEnumerable<TagElement> tags = [new HeaderTag(), new BoldTag(), new ItalicTag(), new MarkedListTag()];

        var singleTagFactory = new SingleTagFactory(tags);
        var doubleTagFactory = new DoubleTagFactory(tags);

        var lineRenderer = new LineRenderer();
        var listRenderer = new ListRenderer();

        var tokenParser = new TokenParser(doubleTagFactory);
        var lineParser = new LineParser(tokenParser, singleTagFactory);
        var renderer = new HtmlRenderer(tags, lineRenderer, listRenderer);

        var processor = new MarkdownProcessor(lineParser, renderer);

        string result = processor.ConvertToHtmlFromString(input);

        Console.WriteLine(result); 
    }
}
