namespace ProtobufText
{

    class UnknownMatcher : Matcher
    {
        public override Token Match(Tokenizer tz)
        {
            int beginIndex = tz.Index;
            tz.Consume();
            return new Token(TokenType.Unknown, tz.Source.Substring( beginIndex, 1) );
        }
    }
}
