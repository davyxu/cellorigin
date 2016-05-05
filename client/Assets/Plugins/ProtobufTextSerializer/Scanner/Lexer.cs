using System.Collections.Generic;

namespace ProtobufText
{
    class Lexer
    {
        IList<Matcher> _tokenmatcher = new List<Matcher>();
        IEnumerator<Token> _tokeniter;
        int _index;

        public void AddMatcher(Matcher matcher)
        {
            _tokenmatcher.Add(matcher);
        }

        public override string ToString()
        {
            if (_tokeniter != null)
                return string.Format("{0} @ {1}",Peek().ToString(), _index);

            return base.ToString();
        }

        public IEnumerable<Token> Tokenize(string source)
        {
            var tz = new Tokenizer(source);

            while( !tz.EOF() )
            {

                foreach (var matcher in _tokenmatcher)
                {
                    var token = matcher.Match(tz);
                    if (token == null)
                    {
                        continue;
                    }

                    // 跳过已经parse部分, 不返回外部
                    if (matcher.IsIgnored)
                        break;


                    yield return token;

                    break;
                }

            }


            yield return new Token(TokenType.EOF, null );
        }

        public void Start( string src )
        {
            _tokeniter = Tokenize(src).GetEnumerator();

            _tokeniter.MoveNext();

        }

        public Token Read( )
        {
            var tk = _tokeniter.Current;

            _tokeniter.MoveNext();
            _index++;

            return tk;
        }

        public Token Peek( )
        {
            return _tokeniter.Current;
        }
    }
}
