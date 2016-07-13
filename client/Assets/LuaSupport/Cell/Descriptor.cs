using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using google.protobuf;

public class Descriptor : ProtoBase
{
    DescriptorProto _def;

    Dictionary<string, FieldDescriptor> _fieldMap = new Dictionary<string, FieldDescriptor>();

    Dictionary<string, Descriptor> _nestedMsgMap = new Dictionary<string, Descriptor>();

    Dictionary<string, EnumDescriptor> _nestedEnumMap = new Dictionary<string, EnumDescriptor>();

    [LuaInterface.NoToLuaAttribute]
    public Descriptor(DescriptorPool pool, ProtoBase parent, DescriptorProto def)        
    {
        _def = def;

        base.Init(pool, parent);

        for ( int i = 0;i< _def.field.Count;i++)
        {
            var fieldDef = _def.field[i];

            var fieldD = new FieldDescriptor( this, fieldDef );

            _fieldMap.Add(fieldDef.name, fieldD);
        }

        for (int i = 0; i < _def.nested_type.Count; i++)
        {
            var nestedDef = _def.nested_type[i];

            var msgD = new Descriptor(pool, this, nestedDef);

            _nestedMsgMap.Add(nestedDef.name, msgD);
        }

        for (int i = 0; i < _def.enum_type.Count; i++)
        {
            var nestedDef = _def.enum_type[i];

            var enumD = new EnumDescriptor(pool, this, nestedDef);

            _nestedEnumMap.Add(nestedDef.name, enumD);
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

    public Descriptor GetNestedMessage(string name)
    {
        Descriptor ret;
        if (_nestedMsgMap.TryGetValue(name, out ret))
        {
            return ret;
        }

        return null;
    }

    public EnumDescriptor GetNestedEnum(string name)
    {
        EnumDescriptor ret;
        if (_nestedEnumMap.TryGetValue(name, out ret))
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
