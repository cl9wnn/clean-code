namespace MarkdownLibrary;

/// <summary>
/// Интерфейс для парсеров разных типов элементов Markdown.
/// </summary>
public interface IFragmentParser
{
    /// <summary>
    /// Парсит строку Markdown и возвращает соответствующий HTML-код.
    /// </summary>
    /// <param name="markdownLine">Одна строка в формате Markdown.</param>
    /// <returns>HTML-код элемента.</returns>
    string ParseLine(string markdownLine);
}
