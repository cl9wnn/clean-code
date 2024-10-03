namespace MarkdownLibrary
{
    public class TokenParser
    {
        private readonly Dictionary<string, TagElement> _tagDictionary;
      
        public TokenParser(Dictionary<string, TagElement> tagDictionary)
        {
            _tagDictionary = tagDictionary;
        }

        public IEnumerable<Token> Process(string markdownText)
        {
            var tokens = new List<Token>();
            var stack = new Stack<string>();
            bool isEscaped = false;

            for (int i = 0; i < markdownText.Length; i++)
            {
                var symbol = markdownText[i].ToString();

                if (symbol == "\\" && !isEscaped)  
                {
                    isEscaped = true;
                    continue;         
                }

                if (CheckIsTag(symbol) && !isEscaped)
                {
                    var currentTag = DetermineTag(markdownText, i);

                    if (stack.Contains(currentTag))
                    {
                        tokens.Add(new Token(_tagDictionary[currentTag], i, isClosing: true));
                        stack.Pop();
                    }
                    else
                    {
                        tokens.Add(new Token(_tagDictionary[currentTag], i));
                        stack.Push(currentTag);
                    }

                    i += currentTag.Length - 1;
                }

                isEscaped = false;
            }

            return tokens;
        }

        //TODO: вынести проверку и опрдеелние символа в отдельную сущность (возможно)
        public bool CheckIsTag(string symbol)   
        {
            return _tagDictionary.ContainsKey(symbol);
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