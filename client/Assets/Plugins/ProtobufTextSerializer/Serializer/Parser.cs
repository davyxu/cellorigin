using System;
using System.Collections.Generic;

namespace ProtobufText
{
    public class Parser
    {
        Lexer _lexer = new Lexer();
        Token _token;

        
        Message _msg;

        Stack<Message> _msgStack = new Stack<Message>();

        public Parser( )
        {
            _lexer.AddMatcher(new NumeralMatcher());
            _lexer.AddMatcher(new StringMatcher());

            _lexer.AddMatcher(new WhitespaceMatcher().Ignore());
            _lexer.AddMatcher(new CommentMatcher().Ignore());


            _lexer.AddMatcher(new TokenMatcher(TokenType.Comma, ":"));
            _lexer.AddMatcher(new TokenMatcher(TokenType.LSqualBracket, "["));
            _lexer.AddMatcher(new TokenMatcher(TokenType.RSqualBracket, "]"));
            _lexer.AddMatcher(new TokenMatcher(TokenType.LBrace, "{"));
            _lexer.AddMatcher(new TokenMatcher(TokenType.RBrace, "}"));


            _lexer.AddMatcher(new IdentifierMatcher());

            _lexer.AddMatcher(new UnknownMatcher());
        }

        public void Merge( string src, Message msg )
        {
            _msg = msg;

            _lexer.Start(src);

            Next();

            string key;

            do
            {
                key = _token.Value;


                switch (_token.Type)
                {
                    case TokenType.Identifier:
                        {
                            Expect(TokenType.Identifier);

                            bool isNormalValue = false;

                            switch (_token.Type)
                            {
                                case TokenType.Comma:
                                    {
                                        isNormalValue = true;
                                    }
                                    break;
                                case TokenType.LBrace:
                                    {
                                        OnMsgBegin(key);

                                        Next();
                                        continue;
                                    }
                            }

                            Next();

                            var valueToken = _token;

                            if (isNormalValue)
                            {
                                OnSetValue(key, valueToken.Value);
                            }

                            Next();

                            if (_token.Type == TokenType.RBrace)
                            {
                                OnMsgEnd();
                                Next();
                            }                
                        }
                        break;
                    case TokenType.RBrace:
                        {
                            OnMsgEnd();
                            Next();
                        }
                        break;
                }

               
                

            } while (_token.Type != TokenType.EOF);
            
        }


        void Expect(TokenType t)
        {
            if (_token.Type != t)
            {
                throw new Exception(string.Format("expect token: {0} @ {1}", t.ToString(), _lexer.ToString() ));
            }

            Next();
        }

        void Error(string str)
        {
            throw new Exception(str);
        }

        public override string ToString()
        {
            return _token.ToString();
        }

        void Next()
        {
            _token = _lexer.Read();
        }





        void OnMsgBegin(string name)
        {
            _msgStack.Push(_msg);
            _msg = _msg.AddMessage(name);
        }


        void OnSetValue(string name, string value )
        {
            try
            {
                _msg.SetValue(name, value);
            }
            catch( Exception ex )
            {
                string err = ex.ToString() + _lexer.ToString();
                throw new Exception(err);
            }
            
        }

        void OnMsgEnd()
        {
            _msg = _msgStack.Pop();
        }

    }
}
