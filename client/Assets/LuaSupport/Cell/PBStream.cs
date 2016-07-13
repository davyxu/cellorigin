using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class PBStream
{
    enum WireType
    {
        WIRETYPE_VARINT = 0,
        WIRETYPE_FIXED64 = 1,
        WIRETYPE_LENGTH_DELIMITED = 2,
        WIRETYPE_START_GROUP = 3,
        WIRETYPE_END_GROUP = 4,
        WIRETYPE_FIXED32 = 5,
    };

    MemoryStream stream = new MemoryStream();

    BinaryWriter writer;

    const int kTagTypeBits = 3;

    public PBStream( )
    {
        writer = new BinaryWriter(stream);
    }

    static uint MakeTag( int fieldNumber, WireType type )
    {
        return (uint)( fieldNumber << kTagTypeBits ) | (uint)type;
    }

    void WriteTag( int fieldNumber, WireType type )
    {
        WriteVariant32(MakeTag(fieldNumber, type));
    }

    void WriteVariant32(UInt32 value)
    {
        while (value >= 0x80)
        {            
            writer.Write((byte)(value | 0x80));

            value >>= 7;            
        }

        writer.Write((byte)value);
    }

    public void WriteInt32( int fieldNumber, int value)
    {
        WriteTag(fieldNumber, WireType.WIRETYPE_VARINT);
        WriteVariant32((UInt32)value);
        writer.Flush();
    }

    public void WriteFloat32(int fieldNumber, float value)
    {


        WriteTag(fieldNumber, WireType.WIRETYPE_FIXED32);
#if FEAT_SAFE
        WriteVariant32(BitConverter.ToInt32(BitConverter.GetBytes(value), 0));
#else
        unsafe
        {
            WriteVariant32(*(UInt32*)&value);
        }
#endif
        writer.Flush();
    }

    public void WriteBool(int fieldNumber, bool value)
    {
        WriteTag(fieldNumber, WireType.WIRETYPE_VARINT);
        WriteVariant32((UInt32)(value ? 1 : 0));
        writer.Flush();
    }

    public void WriteString( int fieldNumber, string value )
    {
        WriteTag(fieldNumber, WireType.WIRETYPE_LENGTH_DELIMITED);
        WriteVariant32((UInt32)value.Length);
        writer.Write(Encoding.UTF8.GetBytes( value) );
        writer.Flush();
    }

    public void WriteMessageHeader(int fieldNumber, int msgSize)
    {
        WriteTag(fieldNumber, WireType.WIRETYPE_LENGTH_DELIMITED);
        WriteVariant32((UInt32)msgSize);
        writer.Flush();
    }

    public override string ToString()
    {
        return Encoding.Default.GetString(stream.ToArray());
    }

    public byte[] ToArray()
    {
        return stream.ToArray();
    }

    public MemoryStream Stream
    {
        get { return stream; }
    }
}
