using System.Security.Cryptography;
using System.Text;
namespace MarkdownLibrary;

    public class HtmlRenderer: IRenderer
    {
        private readonly Dictionary<string, TagElement> _tagDictionary;

        public HtmlRenderer(Dictionary<string, TagElement> tagDictionary)
        {
            _tagDictionary = tagDictionary;
        }

    public string Render(IEnumerable<Line> processedLines)
    {
        var renderedLines = new List<string>();

        foreach (var line in processedLines)
        {
            var renderedLine = RenderLine(line);
            renderedLines.Add(renderedLine);
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

        if (line.IsHeader)
        {
            return RenderHeaderLine(content);
        }

        return content;
    }

    //TODO: Заменить "<h1>" через тег
    private string RenderHeaderLine(string line)
    {
        return $"<h1>{line}</h1>";
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


