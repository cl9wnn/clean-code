using System.Security.Cryptography;
using System.Text;
namespace MarkdownLibrary;

public class HtmlRenderer : IRenderer
{
    private readonly Dictionary<string,TagElement> _tagDictionary;

    public HtmlRenderer(Dictionary<string, TagElement> tagDictionary)
    {
        _tagDictionary = tagDictionary;
    }


    //TODO: возможно сделать наследование от класса Line и упростить логику проверки
    public string Render(IEnumerable<Line> processedLines)
    {
        var renderedLines = new List<string>();
        var isListOpen = false;
        int currentLevel = 0;

        foreach (var line in processedLines)
        {
            var indentLevel = line.IndentLevel;

            if (line.Type is MarkedListTag && !isListOpen)
            {
                isListOpen = true;
                renderedLines.Add("<ul>");
            }

            if (indentLevel > currentLevel)
            {
                renderedLines.Add(new string(' ',4 + indentLevel * 4) + "<ul>");
                currentLevel = indentLevel;
            }
            else if (indentLevel < currentLevel)
            {
                renderedLines.Add(new string(' ',4+ currentLevel * 4) + "</ul>");
                currentLevel = indentLevel;
            }

            if (line.Type is not MarkedListTag && isListOpen)
            {
                isListOpen = false;
                renderedLines.Add("</ul>");
            }

            var renderedLine = RenderLine(line);
            renderedLines.Add(renderedLine);
        }

        if (isListOpen)
        {
            renderedLines.Add("</ul>");
        }

        return RemoveEscapedCharacters(string.Join("\n", renderedLines));
    }

    private string RenderLine(Line line)
    {
        var renderedString = new StringBuilder();

        foreach (var token in line.Tokens)
        {
            var word = token.Word;
            var tagDataList = token.Tags;

            if (tagDataList.Count > 0)
            {
                var renderedWord = ReplaceTags(word, tagDataList);
                renderedString.Append(renderedWord);
            }
            else
            {
                renderedString.Append(word);
            }

            renderedString.Append(' ');
        }

        string content = renderedString.ToString().TrimEnd();

        if (line.Type is HeaderTag headerTag)
        {
            return headerTag.RenderHeaderLine(content);
        }
        else if (line.Type is MarkedListTag listTag)
        {
            return new string(' ', 4 + line.IndentLevel * 8) + listTag.RenderMarkedListLine(content);
        }

        return content;
    }

    private string ReplaceTags(string word, List<TagData> tagDataList)
    {
        var result = new StringBuilder(word);

        foreach (var tagData in tagDataList.OrderByDescending(t => t.Index))
        {
            var tagLength = tagData.Tag.MdTag.Length;
            var replacement = tagData.IsClosing ? tagData.Tag.CloseHtmlTag : tagData.Tag.OpenHtmlTag;

            result.Remove(tagData.Index, tagLength);
            result.Insert(tagData.Index, replacement);
        }

        return result.ToString();
    }


    //TODO: добавить все символы и сделать нормальную проверку
    private string RemoveEscapedCharacters(string text)
    {
        var result = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\\' && i < text.Length - 1)
            {
                var escapedSymbol = text[i + 1].ToString();

                if (_tagDictionary.ContainsKey(escapedSymbol) || escapedSymbol == "#" || escapedSymbol == "\\")
                {
                    continue;
                }
            }
            result.Append(text[i]);
        }

        return result.ToString();
    }
}


