using System.Text;
namespace MarkdownLibrary;

public class HtmlRenderer : IRenderer
{
    private readonly IEnumerable<string> _tags;
    private readonly LineRenderer _lineRenderer;
    private readonly ListRenderer _listRenderer;

    public HtmlRenderer(IEnumerable<string> tags, LineRenderer lineRenderer, ListRenderer listRenderer)
    {
        _tags = tags;
        _lineRenderer = lineRenderer;
        _listRenderer = listRenderer;
    }

    public string Render(IEnumerable<Line> processedLines)
    {
        var renderedLines = new List<string>();

        foreach (var line in processedLines)
        {
            var listRendering = _listRenderer.RenderList(line);
            if (!string.IsNullOrEmpty(listRendering))
            {
                renderedLines.Add(listRendering);
            }

            var renderedLine = _lineRenderer.RenderLine(line);
            renderedLines.Add(renderedLine);
        }

        var closingList = _listRenderer.CloseOpenList();

        if (!string.IsNullOrEmpty(closingList))
        {
            renderedLines.Add(closingList);
        }

        return RemoveEscapedCharacters(string.Join("\n", renderedLines));
    }

    private string RemoveEscapedCharacters(string text)
    {
        var result = new StringBuilder();

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\\' && i < text.Length - 1)
            {
                var escapedSymbol = text[i + 1].ToString();

                if (_tags.Contains(escapedSymbol) || escapedSymbol == "\\")
                {
                    continue;
                }
            }
            result.Append(text[i]);
        }

        return result.ToString();
    }
}


