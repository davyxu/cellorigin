

namespace ProtobufText
{
    class TokenMatcher : Matcher
    {
        string _word;
        TokenType _type;

        public TokenMatcher( TokenType t, string word)
        {
            _word = word;
            _type = t;
        }

        public override Token Match(Tokenizer tz)
        {
            if (tz.CharLeft < _word.Length)
                return null;

            int index = 0;

            foreach( var c in _word )
            {
                if (tz.Peek(index) != c)
                    return null;

                index++;
            }


            tz.Consume(_word.Length);


            return new Token(_type, _word );
        }

    }
}
