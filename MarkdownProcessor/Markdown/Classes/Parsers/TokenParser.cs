namespace MarkdownLibrary;

public class TokenParser : IParser<Token>
{
    private readonly Dictionary<string, TagElement> _tagDictionary;

    public TokenParser(Dictionary<string, TagElement> tagDictionary)
    {
        _tagDictionary = tagDictionary;
    }

    public IEnumerable<Token> Parse(string content)
    {
        var words = content.Split(' ');
        var tokens = new List<Token>();
        var tagStack = new Stack<string>();

        foreach (var word in words)
        {
            var tokenTags = ParseWordTags(word, tagStack);
            tokens.Add(new Token(word, tokenTags));
        }

        RemoveUnclosedTags(tokens, tagStack);
        return tokens;
    }

    private List<TagData> ParseWordTags(string word, Stack<string> tagStack)
    {
        var tokenTags = new List<TagData>();
        bool isEscaped = false;

        if (IsEmptyWord(word))
        {
            return tokenTags;
        }

        for (int i = 0; i < word.Length; i++)
        {
            string symbol = word[i].ToString();

            if (IsEscapeSequence(symbol, ref isEscaped))
            {
                continue;
            }

            if (IsTag(symbol) && !isEscaped && !IsSurroundedByDigits(word, i))
            {
                var currentTag = DetermineTag(word, i);

                if (tagStack.Contains(currentTag))
                {

                    if (HasSpaceBeforeClosingTag(word, currentTag, i) && !IsBoldTagNested(currentTag, tagStack))
                    {
                        tokenTags.Add(CreateClosingTag(currentTag, i));
                        tagStack.Pop();
                    }
                }
                else
                {
                    if (HasSpaceAfterOpenTag(word, currentTag, i) && !IsBoldTagNested(currentTag, tagStack))
                    {
                        tokenTags.Add(CreateOpeningTag(currentTag, i));
                        tagStack.Push(currentTag);
                    }
                }

                i += currentTag.Length - 1;
            }

            isEscaped = false;
        }

        return tokenTags;
    }


    private bool IsTag(string symbol)
    {
        return _tagDictionary.ContainsKey(symbol);
    }

    private string DetermineTag(string word, int index)
    {
        string symbol = word[index].ToString();

        if (index + 1 < word.Length && IsTag(symbol + word[index + 1]))
        {
            return symbol + word[index + 1];
        }

        return symbol;
    }

    private bool IsBoldTagNested(string currentTag, Stack<string> tagStack)
    {
        return tagStack.Contains("_") && currentTag == "__";
    }

    private bool IsEmptyWord(string word)
    {
        return word.All(c => IsTag(c.ToString()));
    }

    private bool HasSpaceAfterOpenTag(string word, string currentTag, int index)
    {
        return index + currentTag.Length < word.Length && word[index + currentTag.Length] != ' ';
    }

    private bool HasSpaceBeforeClosingTag(string word, string currentTag, int index)
    {
        return index > 0 && word[index - currentTag.Length] != ' ';
    }

    private bool IsEscapeSequence(string symbol, ref bool isEscaped)
    {
        if (symbol == "\\" && !isEscaped)
        {
            isEscaped = true;
            return true;
        }

        return false;
    }

    private bool IsSurroundedByDigits(string word, int index)
    {
        bool isBeforeDigit = index > 0 && char.IsDigit(word[index - 1]);
        bool isAfterDigit = index + 1 < word.Length && char.IsDigit(word[index + 1]);

        return isBeforeDigit || isAfterDigit;
    }

    private void RemoveUnclosedTags(List<Token> tokens, Stack<string> tagStack)
    {
        while (tagStack.Count > 0)
        {
            var unclosedTag = tagStack.Pop();

            if (!_tagDictionary.TryGetValue(unclosedTag, out var tagToRemove))
            {
                continue;
            }

            foreach (var token in tokens)
            {
                if (token.Tags.Any(t => t.Tag == tagToRemove && t.Tag.IsDoubleTag))
                {
                    token.Tags.RemoveAll(t => t.Tag == tagToRemove && t.Tag.IsDoubleTag);
                }
            }
        }
    }
  

    private TagData CreateOpeningTag(string tag, int index)
    {
        return new TagData(_tagDictionary[tag], index);
    }

    private TagData CreateClosingTag(string tag, int index)
    {
        return new TagData(_tagDictionary[tag], index, isClosing: true);
    }
}
