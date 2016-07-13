using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using google.protobuf;

public class FileDescriptor : ProtoBase
{
    FileDescriptorProto _def;

    public FileDescriptor(ProtoBase parent, FileDescriptorProto def)        
    {   
        _def = def;

        base.Init(parent);
    }

    protected override string GetRawName()
    {
        return _def.package;
    }
}
