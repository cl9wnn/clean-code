namespace MarkdownLibrary;

public interface ILineTag
{ 
    string RenderLine(string content, int indentLevel);
}
