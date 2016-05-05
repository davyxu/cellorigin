using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtobufText
{
    public class Serializer
    {
        static Printer _printer = new Printer();

        public static string Serialize<T>( T instance )
        {            
            return _printer.Print(instance);
        }

        static Parser _parser = new Parser();

        public static T Deserialize<T>(string text )
        {
            var ins = Activator.CreateInstance<T>();
            _parser.Merge(text, new Message(ins));

            return ins;
        }
    }
}
