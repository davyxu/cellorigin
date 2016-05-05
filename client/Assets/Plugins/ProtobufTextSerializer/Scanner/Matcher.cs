

namespace ProtobufText
{
    abstract class Matcher
    {
        bool _ignore;

        public abstract Token Match(Tokenizer tz);
        public Matcher Ignore( )
        {
            _ignore = true;
            return this;
        }

        public bool IsIgnored
        {
            get { return _ignore; }
        }
    }
}
