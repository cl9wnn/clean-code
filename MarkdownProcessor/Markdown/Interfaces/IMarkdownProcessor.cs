namespace MarkdownLibrary;

/// <summary>
/// Публичный контракт для конвертации всего текста разметки Markdown в HTML.
/// </summary>
public interface IMarkdownProcessor
{
    /// <summary>
    /// Конвертирует данный ему текст из формата Markdown в HTML.
    /// </summary>
    /// <param name="markdownText">Текст в формате Markdown.</param>
    /// <returns>HTML-код.</returns>
    string ConvertToHtml(string markdownText);
}
