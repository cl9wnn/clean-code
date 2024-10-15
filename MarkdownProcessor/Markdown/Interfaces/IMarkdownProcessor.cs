namespace MarkdownLibrary;

/// <summary>
/// Публичный контракт для конвертации всего текста разметки Markdown в HTML.
/// </summary>
public interface IMarkdownProcessor
{
    /// <summary>
    /// Конвертирует данный ему текст из файла формата Markdown в HTML.
    /// </summary>
    /// <param name="filePath">Путь до .md файла.</param>
    /// <returns>HTML-код.</returns>
    string ConvertToHtmlFromFile(string filePath);

    /// <summary>
    /// Конвертирует данный ему текст из строки в формате Markdown в HTML.
    /// </summary>
    /// <param name="markdownText">Текст в формате Markdown.</param>
    /// <returns>HTML-код.</returns>
    string ConvertToHtmlFromString(string markdownText);

}
