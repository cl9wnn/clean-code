using System.Reflection.Metadata;

namespace MarkdownLibrary;

public class Program
{ 
    public static void Main(string[] args)
    {
        string input = " * element";

        var processor = new MarkdownProcessor();

        Console.WriteLine(processor.ConvertToHtmlFromString(input));
    }
}
