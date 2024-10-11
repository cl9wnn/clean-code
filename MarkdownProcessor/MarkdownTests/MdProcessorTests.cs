using System.Diagnostics;

namespace MarkdownLibrary.Tests;

public class MdProcessorTests
{

    private readonly MarkdownProcessor _processor;

    public MdProcessorTests()
    {
        _processor = new MarkdownProcessor();
    }


    //TODO: Make tests for time complexity(O(n))

    [Theory]
    [InlineData("__text__", "<strong>text</strong>")]
    [InlineData("__bold text__", "<strong>bold text</strong>")]
    public void DoubleUnderscore_ShouldConvertToStrong(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("_text_", "<em>text</em>")]
    [InlineData("_italic text_", "<em>italic text</em>")]
    public void SingleUnderscore_ShouldConvertToEmphasis(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(@"\# Header", "# Header")]
    [InlineData(@"\_text\_", "_text_")]
    [InlineData(@"\_\_text\_\_", "__text__")]
    public void EscapedTags_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(@"te\xt", @"te\xt")]
    [InlineData(@"te\xt \with escaping\", @"te\xt \with escaping\")]
    public void Escaping_ShouldWorkOnlyWithTags(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(@"\\_text_", @"\<em>text</em>")]
    [InlineData(@"\\__text__", @"\<strong>text</strong>")]
    public void EscapingSymbol_ShouldScreenEscapingSymbol(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("__emphasis _text_ convert__", "<strong>emphasis <em>text</em> convert</strong>")]
    public void NestedEmphasisTag_ShouldConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("_bold __text__ doesnt convert_", "<em>bold __text__ doesnt convert</em>")]
    public void NestedStrongTag_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("text_12_3", "text_12_3")]
    [InlineData("__text123__", "__text123__")]
    [InlineData("text_123_example", "text_123_example")]
    public void TagsInsideWordsWithNumbers_ShouldNotConvert(string input, string expected)
    { 
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("_sta_rt", "<em>sta</em>rt")]
    [InlineData("cen__te__r", "cen<strong>te</strong>r")]
    [InlineData("en_d_", "en<em>d</em>")]
    [InlineData("_sta_rt_exa_mple", "<em>sta</em>rt<em>exa</em>mple")]
    [InlineData("_sta_rt_exa_mple_", "<em>sta</em>rt<em>exa</em>mple_")]
    public void TagsInsideWords_ShouldConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("diff_erent wo_rd", "diff_erent wo_rd")]
    [InlineData("diff__erent wo__rd", "diff__erent wo__rd")]
    [InlineData("diff_erent word_", "diff_erent word_")]
    public void TagsInsideWordsInDifferentWords_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("__word_", "__word_")]
    [InlineData("_word__", "_word__")]
    [InlineData("__Dont convert_", "__Dont convert_")]
    [InlineData("_Dont convert__", "_Dont convert__")]

    public void UnmatchedSymbols_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("example_ text_", "example_ text_")]
    [InlineData("example__ long line bold text__", "example__ long line bold text__")]

    public void NoWhitespaceAfterTag_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("_example _text", "_example _text")]
    [InlineData("_example long line bold _text", "_example long line bold _text")]
    public void NoWhitespaceBeforeTag_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("__bold _text__ and emphasis_", "__bold _text__ and emphasis_")]
    [InlineData("_bold __text_ and emphasis__", "_bold __text_ and emphasis__")]
    [InlineData("_bold __text and_ emphasis__", "_bold __text and_ emphasis__")]

    public void CrossingUnderscores_ShouldNotConvert(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("____", "____")]
    [InlineData("__ __", "__ __")]
    [InlineData("__", "__")]
    [InlineData("text and __ another _text_", "text and __ another <em>text</em>")]
    public void EmptyStringBetweenTags_ShouldRemainAsUnderscores(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("#Header text", "#Header text")]
    [InlineData("# Header text", "<h1>Header text</h1>")]
    public void SingleCharp_ShouldConvertToHeader(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("# __Заголовок__", "<h1><strong>Заголовок</strong></h1>")]
    [InlineData("# _Заголовок_", "<h1><em>Заголовок</em></h1>")]
    [InlineData("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
    public void HeaderWithAnotherTags_ShouldWork(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Default Text\nAnother Text", "Default Text\nAnother Text")]
    [InlineData("# Header\nDefault Text\nAnother Text", "<h1>Header</h1>\nDefault Text\nAnother Text")]
    public void Lines_ShouldDividedCorrectrly(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("*Element", "*Element")]
    [InlineData("* Element", "<ul>\n    <li>Element</li>\n</ul>")]
    [InlineData("* Element 1\n* Element 2\n* Element 3", "<ul>\n    <li>Element 1</li>\n    <li>Element 2</li>\n    <li>Element 3</li>\n</ul>")]
    public void MarkedList_ShouldWork(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("* Element", "<ul>\n    <li>Element</li>\n</ul>")]
    [InlineData("- Element", "<ul>\n    <li>Element</li>\n</ul>")]
    [InlineData("+ Element", "<ul>\n    <li>Element</li>\n</ul>")]
    [InlineData("* Element 1\n- Element 2\n+ Element 3", "<ul>\n    <li>Element 1</li>\n    <li>Element 2</li>\n    <li>Element 3</li>\n</ul>")]
    public void MarkedList_ShouldSupportAllListSymbols(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData("* _Italic element_\n* _Italic element_", "<ul>\n    <li><em>Italic element</em></li>\n    <li><em>Italic element</em></li>\n</ul>")]
    [InlineData("* __Bold element__\n* __Bold element__", "<ul>\n    <li><strong>Bold element</strong></li>\n    <li><strong>Bold element</strong></li>\n</ul>")]
    public void MarkedList_ShouldConvertAllTags(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("+ Пункт A\n+ Пункт B\n  + Подпункт 1\n  + Подпункт 2\n+ Пункт C", "<ul>\n    <li>Пункт A</li>\n    <li>Пункт B</li>\n    <ul>\n        <li>Подпункт 1</li>\n        <li>Подпункт 2</li>\n    </ul>\n    </li>\n    <li>Пункт C</li>\n</ul>\n ")]
    public void MarkedList_ShouldSupportNestedLists(string input, string expected)
    {
        string result = _processor.ConvertToHtml(input);

        Assert.Equal(expected, result);
    }
}

