namespace MarkdownLibrary.Tests;

public class MdProcessorTests
{

    private readonly MarkdownProcessor _processor;

    public MdProcessorTests()
    {
        _processor = new MarkdownProcessor();
    }


    [Fact]
    public void SingleCharp_ShouldConvertToHeader()
    {
        var input = "#Header text";
        var expected = "<h1>Header text</h1>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void DoubleUnderscore_ShouldConvertToStrong()
    {
        // Arrange
        var input = "__bold text__";
        var expected = "<strong>bold text</strong>";

        string result = _processor.ConvertToHtml(input);

        //Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SingleUnderscore_ShouldConvertToEmphasis()
    {
        var input = "_italic text_";
        var expected = "<em>italic text</em>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EscapedHeader_ShouldNotConvert()
    {
        string input = @"\#Вот это заголовок";
        string expected = @"#Вот это заголовок";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EscapedItalicText_ShouldNotConvert()
    {
        string input = @"\_Вот это\_ не должно выделиться тегом <em>.";
        string expected = "_Вот это_ не должно выделиться тегом <em>.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EscapedBoldText_ShouldNotConvert()
    {
        string input = @"\_\_Вот это\_\_ не должно выделиться тегом <strong>.";
        string expected = "__Вот это__ не должно выделиться тегом <strong>.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escaping_Correctly()
    {
        string input = @"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\";
        string expected = @"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Escaping_ShouldWorkCorrectly()
    {
        string input = @"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_";
        string expected = @"Символ экранирования тоже можно экранировать: \<em>вот это будет выделено тегом</em>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void NestedEmphasisAndStrong_ShouldWorkCorrectly()
    {
        string input = "__двойное выделение _одинарное_ тоже__ работает.";
        string expected = "<strong>двойное выделение <em>одинарное</em> тоже</strong> работает.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void SingleUnderscoreInsideDoubleUnderscore_ShouldNotWork()
    {
        string input = "_одинарное __двойное__ не_ работает.";
        string expected = "<em>одинарное __двойное__ не</em> работает.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void UnderscoresInsideWordsWithNumbers_ShouldNotConvert()
    {
        string input = "текст c цифрами_12_3 не считаются выделением.";
        string expected = "текст c цифрами_12_3 не считаются выделением.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void UnderscoresInsideWords_ShouldConvert()
    {
        string input = "и в _нач_але, и в сер_еди_не, и в кон_це._";
        string expected = "и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void UnmatchedSymbols_ShouldNotConvert()
    {
        string input = "__непарные_ символы не считаются выделением.";
        string expected = "__непарные_ символы не считаются выделением.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void NoWhitespaceAfterUnderscore_ShouldNotConvert()
    {
        string input = "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением";
        string expected = "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void NoWhitespaceBeforeUnderscore_ShouldNotConvert()
    {
        string input = "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются окончанием выделения ";
        string expected = "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются окончанием выделения ";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void CrossingUnderscores_ShouldNotConvert()
    {
        string input = "__пересечения _двойных__ и одинарных_ подчерков__.";
        string expected = "__пересечения _двойных__ и одинарных_ подчерков__.";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void EmptyStringBetweenUnderscores_ShouldRemainAsUnderscores()
    {
        string input = "если внутри подчерков пустая строка ____. ";
        string expected = "если внутри подчерков пустая строка ____. ";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void HeadingConversion_ShouldWork()
    {
        string input = "#Заголовок __с _разными_ символами__";
        string expected = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";

        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }
}

