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

            if (tokenTags.Count == 1 && IsTagInsideWord(tokenTags.First(), word))
            {
                tokenTags.Clear();
                if (tagStack.Count > 0)
                {
                    tagStack.Pop();
                }
            }

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


            if (IsTag(symbol) && !isEscaped)
            {
                var currentTag = DetermineTag(word, i);

                if (ContainsDigitsInsideTag(word, i, currentTag))
                {
                    continue;
                }

                if (tagStack.Contains(currentTag))
                {
                    if (NotHasSpaceBeforeClosingTag(word, currentTag, i) && !IsBoldTagNested(currentTag, tagStack))
                    {
                        tokenTags.Add(CreateClosingTag(currentTag, i));
                        tagStack.Pop();
                    }
                }
                else
                {

                    if (NotHasSpaceAfterOpenTag(word, currentTag, i) && !IsBoldTagNested(currentTag, tagStack))
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

    private bool ContainsDigitsInsideTag(string word, int tagPosition, string tag)
    {
        int start = word.IndexOf(tag, tagPosition);
        int end = word.LastIndexOf(tag);

        if (start != -1 && end != -1 && start < end)
        {
            var innerContent = word.Substring(start + tag.Length, end - (start + tag.Length));

            return innerContent.All(char.IsDigit);
        }

        return false;
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

    //TODO: поменять названия
    private bool NotHasSpaceAfterOpenTag(string word, string currentTag, int index)
    {
        return index + currentTag.Length < word.Length && word[index + currentTag.Length] != ' ';
    }

    private bool NotHasSpaceBeforeClosingTag(string word, string currentTag, int index)
    {
        return index >= currentTag.Length && word[index - currentTag.Length] != ' ';
    }

    private bool IsTagInsideWord(TagData tokenTag, string word)
    {
        return tokenTag.Index != 0 && tokenTag.Index != word.Length - tokenTag.Tag.MdTag.Length;
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
