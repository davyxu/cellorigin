using System;

namespace ProtobufText
{
    class NumeralMatcher : Matcher
    {
        public override Token Match(Tokenizer tz)
        {

            if (!Char.IsDigit(tz.Current) && tz.Current != '-' )
                return null;

            int beginIndex = tz.Index;


            do
            {
                tz.Consume();

            } while (char.IsDigit(tz.Current) || tz.Current == '.');


            return new Token(TokenType.Number, tz.Source.Substring( beginIndex, tz.Index - beginIndex) );
        }

    }
}
