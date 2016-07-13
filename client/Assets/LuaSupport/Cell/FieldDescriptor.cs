using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using google.protobuf;

public class FieldDescriptor
{
    public FieldDescriptorProto Define { get; private set; }

    public FieldDescriptor( FieldDescriptorProto def)        
    {
        Define = def;
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
        get { return null;  }
    }

    public bool IsRepeated
    {
        get { return Define.label == FieldDescriptorProto.Label.LABEL_REPEATED; }
    }
}
