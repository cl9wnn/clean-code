namespace MarkdownLibrary.Tests;

public class MdProcessorTests
{
    [Fact]
    public void ParseLine_InputWithDoubleBoldTag_ShouldReturnStrongTag()
    {
        // Arrange
        var input = "__bold text__";
        var expected = "<strong>bold text</strong>";

        var textParser = new DoubleTagParser(new BoldTextTag());

        //Act
        string result = textParser.ParseLine(input);

        //Assert
        Assert.Equal(expected, result);
    }
}