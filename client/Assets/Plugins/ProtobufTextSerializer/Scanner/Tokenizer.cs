
namespace ProtobufText
{
    class Tokenizer
    {
        string _source;
       
        public Tokenizer(string src)
        {
            _source = src;
        }

        public override string ToString()
        {
            return Current.ToString();
        }

        public int Index { get; set; }

        public string Source { get { return _source; } }

        public char Current
        {
            get {
                if (EOF(0))
                    return '\0';

                return _source[Index];
            }
        }

        public int CharLeft
        {
            get { return _source.Length - Index;  }
        }

        public char Peek(int offset)
        {
            if (EOF(offset))
                return '\0';

            return _source[Index + offset ];
        }

        public int Line { get; set; }


        public void Consume( int count = 1 )
        {
            Index+= count ;
        }

        public bool EOF(int offset = 0)
        {
            return Index + offset >= _source.Length;
        }

        public void IncLine( )
        {
            Line++;
        }
    }
}
