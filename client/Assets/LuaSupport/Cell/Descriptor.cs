using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using google.protobuf;

public class Descriptor : ProtoBase
{
    DescriptorProto _def;

    Dictionary<string, FieldDescriptor> _fieldMap = new Dictionary<string, FieldDescriptor>();

    public Descriptor(ProtoBase parent, DescriptorProto def)        
    {
        _def = def;

        base.Init(parent);

        for ( int i = 0;i< _def.field.Count;i++)
        {
            var fieldDef = _def.field[i];

            var fieldD = new FieldDescriptor( fieldDef );

            _fieldMap.Add(fieldDef.name, fieldD);
        }
    }

    public FieldDescriptor GetFieldByName( string name )
    {
        FieldDescriptor ret;
        if (_fieldMap.TryGetValue(name, out ret))
        {
            return ret;
        }

        return null;
    }

    protected override string GetRawName()
    {
        return _def.name;
    }
}
