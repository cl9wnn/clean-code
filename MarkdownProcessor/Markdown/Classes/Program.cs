using System.Reflection.Metadata;

namespace MarkdownLibrary;

public class Program
{ 
    public static void Main(string[] args)
    {
        string input = "__Bold #text__ and  _italic text_";

        var processor = new MarkdownProcessor();

        Console.WriteLine(processor.ConvertToHtmlFromString(input));
    }
}
