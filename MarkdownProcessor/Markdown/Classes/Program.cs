namespace MarkdownLibrary;

public class Program
{ 
    public static void Main(string[] args)
    {
        string input = "+ Item 1\n* Item 2\n  * Sub-item 1\n+ Item 3";
     //   Console.WriteLine("<ul>\n    <li>Item 1</li>\n    <li>Item 2</li>\n        <ul>\n            <li>Sub-item 1</li>\n        </ul>\n    <li>Item 3</li>\n        <ul>\n            <li>Sub-item 2</li>\n        </ul>\n</ul>");
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
