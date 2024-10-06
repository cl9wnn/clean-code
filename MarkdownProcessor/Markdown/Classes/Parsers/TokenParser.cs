namespace MarkdownLibrary;

    public class TokenParser: IParser<Token>
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
        private List<TagData> ParseWordTags(string word, Stack<string> tagStack)
        {
                var tokenTags = new List<TagData>();
                bool isEscaped = false;


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

                            if (tagStack.Contains(currentTag))
                            {
                                tokenTags.Add(CreateClosingTag(currentTag, i));
                                tagStack.Pop();
                            }
                            else
                            {
                                tokenTags.Add(CreateOpeningTag(currentTag, i));
                                tagStack.Push(currentTag);
                            }

                            i += currentTag.Length - 1;
                        }

                        isEscaped = false;
                    }

                     return tokenTags;
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

            private TagData CreateOpeningTag(string tag, int index)
            {
                return new TagData(_tagDictionary[tag], index);
            }

            private TagData CreateClosingTag(string tag, int index)
            {
                return new TagData(_tagDictionary[tag], index, isClosing: true);
            }
    }
