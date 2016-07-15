using System;
using System.Text;
using Google.Protobuf;

public class PBStreamReader
{
    CodedInputStream _stream;

    const int kTagTypeBits = 3;

    // 从C#端初始化
    [LuaInterface.NoToLuaAttribute]
    public PBStreamReader(byte[] bytes )
    {
        _stream = new CodedInputStream(bytes);
    }

    // 从Lua端放入
    public PBStreamReader(string str)
    {
        _stream = new CodedInputStream( Encoding.Default.GetBytes(str) );
    }

    public FieldDescriptor ReadField( Descriptor msgD )
    {
        if (msgD == null)
            return null;

        try
        {
            uint tag;
            while ((tag = _stream.ReadTag()) != 0)
            {
                var fieldNumber = (int)(tag >> kTagTypeBits);

                var fd = msgD.GetFieldByFieldNumber(fieldNumber);
                if (fd != null)
                {
                    return fd;
                }

                _stream.SkipLastField();
            }
        }
        catch( Exception )
        {            
            return null;
        }

        return null;
    }
    public bool ReadInt32(out int value)
    {
        
        try
        {
            value = _stream.ReadInt32();
        }
        catch( Exception )
        {
            value = 0;
            return false;
        }
        
        return true;
    }

    public bool ReadUInt32(out uint value)
    {

        try
        {
            value = _stream.ReadUInt32();
        }
        catch (Exception)
        {
            value = 0;
            return false;
        }

        return true;
    }

    public bool ReadInt64(out string value)
    {

        try
        {
            var number = _stream.ReadInt64();
            value = number.ToString();            
        }
        catch (Exception)
        {
            value = default(string);
            return false;
        }

        return true;
    }

    public bool ReadUInt64(out string value)
    {

        try
        {
            var number = _stream.ReadUInt64();
            value = number.ToString();
        }
        catch (Exception)
        {
            value = default(string);
            return false;
        }

        return true;
    }


    public bool ReadFloat(out float value)
    {
        try
        {
            value = _stream.ReadFloat();
        }
        catch (Exception)
        {
            value = 0;
            return false;
        }

        return true;
    }

    public bool ReadBool(out bool value )
    {
        try
        {
            value = _stream.ReadBool();
        }
        catch (Exception)
        {
            value = false;
            return false;
        }

        return true;
    }

    public bool ReadString(out string value )
    {
        try
        {
            value = _stream.ReadString();
        }
        catch (Exception)
        {
            value = default(string);
            return false;
        }
        
        return true;
    }

    public bool BeginMessage(out int limit )
    {
        try
        {
            return _stream.BeginMessage(out limit);
        }
        catch (Exception)
        {
            limit = 0;
            return false;
        }
    }

    public bool EndMessage(int limit)
    {
        try
        {
            return _stream.EndMessage(limit);
        }
        catch (Exception)
        {
            return false;
        }
    }
}
