using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using google.protobuf;
using Google.Protobuf;

public class LuaPB
{
    public static string GetTestData( )
    {
        var p = new tutorial.Person();
        p.name = "hello";
        p.test.Add(1);
        p.test.Add(2);
        p.phone.Add(new tutorial.PhoneNumber
        {
            number = "789",
            type = tutorial.PhoneType.WORK,
        });

        p.phone.Add(new tutorial.PhoneNumber
        {
            number = "456",

            type = tutorial.PhoneType.HOME,
        });

        var stream = new MemoryStream();
        ProtoBuf.Serializer.Serialize(stream, p);

        var s = Encoding.Default.GetString(stream.ToArray());        

        return s;
    }

    public static void TestStream( PBStreamWriter s )
    {
        var p = new tutorial.Person();
        p.name = "hello";
        p.test.Add(1);
        p.test.Add(2);
        p.phone.Add(new tutorial.PhoneNumber
        {
            number = "789",
            type = tutorial.PhoneType.WORK,
        });

        p.phone.Add(new tutorial.PhoneNumber
        {
            number = "456",

            type = tutorial.PhoneType.HOME,
        });

        var stream = new MemoryStream();
        ProtoBuf.Serializer.Serialize(stream, p);

        s.Stream.Position = 0;
        var data = ProtoBuf.Serializer.Deserialize<tutorial.Person>(s.Stream);
        var compareStream = new MemoryStream();
        ProtoBuf.Serializer.Serialize(compareStream, data);


        if (CompareMemoryStreams(stream, compareStream))
        {
            int a = 1;
        }
    }

    private static bool CompareMemoryStreams(MemoryStream ms1, MemoryStream ms2)
    {
        if (ms1.Length != ms2.Length)
            return false;
        ms1.Position = 0;
        ms2.Position = 0;

        var msArray1 = ms1.ToArray();
        var msArray2 = ms2.ToArray();

        return msArray1.SequenceEqual(msArray2);
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
        return CodedOutputStream.ComputeTagSize(fieldNumber);        
    }

    
    public static int VarintSize32( uint value )
    {
        return CodedOutputStream.ComputeRawVarint32Size(value);
    }

    public static int Int32Size( int value )
    {
        return CodedOutputStream.ComputeInt32Size(value);
    }    
}
