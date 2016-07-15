using System.IO;
using System.Text;
using Google.Protobuf;
using System;



public class PBStreamWriter
{

    MemoryStream _data = new MemoryStream();

    CodedOutputStream _stream;

    const int kTagTypeBits = 3;

    public PBStreamWriter( )
    {
        _stream = new CodedOutputStream(_data);        
    }
    public void WriteInt32( int fieldNumber, int value)
    {        
        _stream.WriteTag(fieldNumber, WireFormat.WireType.Varint);
        _stream.WriteInt32(value);
    }

    public void WriteUInt32(int fieldNumber, uint value)
    {
        _stream.WriteTag(fieldNumber, WireFormat.WireType.Varint);
        _stream.WriteUInt32(value);
    }

    public void WriteInt64(int fieldNumber, string value)
    {
        _stream.WriteTag(fieldNumber, WireFormat.WireType.Varint);

        long v = 0;
        Int64.TryParse(value, out v);
        
        _stream.WriteInt64(v); 
    }

    public void WriteUInt64(int fieldNumber, string value)
    {
        _stream.WriteTag(fieldNumber, WireFormat.WireType.Varint);

        ulong v = 0;
        UInt64.TryParse(value, out v);

        _stream.WriteUInt64(v); 
    }

    public void WriteFloat32(int fieldNumber, float value)
    {
        _stream.WriteTag(fieldNumber, WireFormat.WireType.Fixed32);
        _stream.WriteFloat(value);
    }

    public void WriteBool(int fieldNumber, bool value)
    {
        _stream.WriteTag(fieldNumber, WireFormat.WireType.Varint);
        _stream.WriteBool(value);        
    }

    public void WriteString( int fieldNumber, string value )
    {
        _stream.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
        _stream.WriteString(value);        
    }

    public void WriteMessageHeader(int fieldNumber, int msgSize)
    {
        _stream.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
        _stream.WriteLength(msgSize);        
    }

    public void Flush( )
    {
        _stream.Flush();        
    }

    public override string ToString()
    {
        return Encoding.Default.GetString(_data.ToArray());
    }

    public byte[] ToArray()
    {
        return _data.ToArray();
    }

    public MemoryStream Stream
    {
        get { return _data; }
    }
}
