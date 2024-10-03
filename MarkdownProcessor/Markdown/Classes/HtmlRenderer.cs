using System.Text;

namespace MarkdownLibrary
{
    public class HtmlRenderer: IRenderer
    {
        private readonly Dictionary<string, TagElement> _tagDictionary;

        public HtmlRenderer(Dictionary<string, TagElement> tagDictionary)
        {
            _tagDictionary = tagDictionary;
        }

        public string Render(string markdownText, IEnumerable<Token> tokens)
            {
                var result = new StringBuilder(markdownText);
                var offset = 0;

                foreach (var token in tokens)
                {
                    string replacement = token.IsClosing ? token.Tag.CloseHtmlTag : token.Tag.OpenHtmlTag;
                    result.Remove(token.StartIndex + offset, token.Tag.MdTag.Length);
                    result.Insert(token.StartIndex + offset, replacement);

                    offset += replacement.Length - token.Tag.MdTag.Length;
                }
                return RemoveEscapedCharacters(result.ToString());
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

    }
