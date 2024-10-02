namespace MarkdownLibrary
{
    public class TokenParser
    {
        private static readonly Dictionary<string, TagElement> TagsDictionary = new()
        {
            { "_", new ItalicTag() },
            { "__", new BoldTag() },
            { "#", new HeaderTag() }
        };

        public IEnumerable<Token> Process(string markdownText)
        {
            var tokens = new List<Token>();
            var stack = new Stack<string>();

            for (int i = 0; i < markdownText.Length; i++)
            {
                var symbol = markdownText[i].ToString();

                if (CheckIsTag(symbol))
                {
                    var currentTag = DetermineTag(markdownText, i);

                    if (stack.Contains(currentTag))
                    {
                        tokens.Add(new Token(TagsDictionary[currentTag], i, isClosing: true));
                        stack.Pop();
                    }
                    else
                    {
                        tokens.Add(new Token(TagsDictionary[currentTag], i));
                        stack.Push(currentTag);
                    }

                    i += currentTag.Length - 1;
                }
            }

            return tokens;
        }

        public bool CheckIsTag(string symbol)
        {
            return TagsDictionary.ContainsKey(symbol);
        }

        private string DetermineTag(string markdownText, int index)
        {
            var symbol = markdownText[index].ToString();

            if (index + 1 < markdownText.Length && CheckIsTag(symbol + markdownText[index + 1]))
            {
                return symbol + markdownText[index + 1];
            }

            return symbol; 
        }
    }

   
}