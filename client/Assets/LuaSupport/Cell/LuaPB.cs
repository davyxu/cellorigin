using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using google.protobuf;

public class LuaPB
{
    public static string GetTestData( )
    {
        var p = new tutorial.Person();
        p.name = "hello";

        var stream = new MemoryStream();

        ProtoBuf.Serializer.Serialize(stream, p);



        var s = Encoding.Default.GetString(stream.ToArray());        

        return s;
    }

    static DescriptorPool _pool = new DescriptorPool();

    public static void RegisterFile( string filename )
    {        
        using( FileStream reader = File.OpenRead(filename) )
        {
            var data = ProtoBuf.Serializer.Deserialize<FileDescriptorSet>(reader);

            reader.Dispose();

            _pool.Init(data);
        }

        var msg = _pool.GetMessage("tutorial.Person");

    }

    public static DescriptorPool GetPool( )
    {
        return _pool;
    }


    public static int TagSize( int fieldNumber )
    {
        return VarintSize32((uint)(fieldNumber << kTagTypeBits));
    }


    const int kTagTypeBits = 3;
    public static int VarintSize32( uint value )
    {
        if (value < (1 << 7))
        {
            return 1;
        }
        else if (value < (1 << 14))
        {
            return 2;
        }
        else if (value < (1 << 21))
        {
            return 3;
        }
        else if (value < (1 << 28))
        {
            return 4;
        }

        return 5;
    }

    public static int Int32Size( int value )
    {
        if (value < 0)
            return 10;

        return VarintSize32((uint)value);
    }    
}
