using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using google.protobuf;

public class FieldDescriptor
{
    public FieldDescriptorProto Define { get; private set; }

    Descriptor _d;

    [LuaInterface.NoToLuaAttribute]
    public FieldDescriptor( Descriptor d, FieldDescriptorProto def)        
    {
        Define = def;
        _d = d;
    }
    
    public int Type
    {
        get { return (int)Define.type; }
    }

    public int Number
    {
        get { return Define.number; }
    }

    public Descriptor MessageType
    {
        get { 

            if ( Define.type_name.Length > 0 && Define.type_name[0] == '.' )
            {
                return _d.Pool.GetMessage(Define.type_name.Substring(1));
            }

            return _d.GetNestedMessage(Define.type_name);
        }
    }


    public EnumDescriptor EnumType
    {
        get
        {

            if (Define.type_name.Length > 0 && Define.type_name[0] == '.')
            {
                return _d.Pool.GetEnum(Define.type_name.Substring(1));
            }

            return _d.GetNestedEnum(Define.type_name);
        }
    }

    public bool IsRepeated
    {
        get { return Define.label == FieldDescriptorProto.Label.LABEL_REPEATED; }
    }
}
