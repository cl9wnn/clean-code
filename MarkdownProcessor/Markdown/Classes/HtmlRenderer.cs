using System.Text;

namespace MarkdownLibrary
{
    public class HtmlRenderer: IRenderer
    {
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

                return result.ToString();
            }
        }
    }
