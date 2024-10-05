using System.Text;
namespace MarkdownLibrary;

    public class HtmlRenderer: IRenderer
    {
        private readonly Dictionary<string, TagElement> _tagDictionary;

        public HtmlRenderer(Dictionary<string, TagElement> tagDictionary)
        {
            _tagDictionary = tagDictionary;
        }

        public string Render(IEnumerable<Token> tokens)
        {
            var renderedString = new StringBuilder();

            foreach (var token in tokens)
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

                renderedString.Append(" ");
            }

            return RemoveEscapedCharacters(renderedString.ToString().TrimEnd());
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

                        if (_tagDictionary.ContainsKey(escapedSymbol) || escapedSymbol == "\\")
                        {
                            continue;
                        }
                    }
                    result.Append(text[i]);
                }

                return result.ToString();
            }
    }


