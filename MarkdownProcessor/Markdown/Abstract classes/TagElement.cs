namespace MarkdownLibrary;
public abstract class TagElement
{
    public abstract string MdTag { get; }
    public abstract string OpenHtmlTag { get; }
    public abstract string CloseHtmlTag { get; }
    public abstract bool IsDoubleTag { get; }
}
