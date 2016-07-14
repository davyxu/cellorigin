using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using google.protobuf;

public class Descriptor : ProtoBase
{
    public DescriptorProto Define { get; private set; }

    Dictionary<string, FieldDescriptor> _fieldByNameMap = new Dictionary<string, FieldDescriptor>();
    Dictionary<int, FieldDescriptor> _fieldByFieldNumberMap = new Dictionary<int, FieldDescriptor>();

    Dictionary<string, Descriptor> _nestedMsgMap = new Dictionary<string, Descriptor>();

    Dictionary<string, EnumDescriptor> _nestedEnumMap = new Dictionary<string, EnumDescriptor>();

    [LuaInterface.NoToLuaAttribute]
    public Descriptor(DescriptorPool pool, ProtoBase parent, DescriptorProto def)        
    {
        Define = def;

        base.Init(pool, parent);

        for ( int i = 0;i< def.field.Count;i++)
        {
            var fieldDef = def.field[i];

            var fieldD = new FieldDescriptor( this, fieldDef );

            _fieldByNameMap.Add(fieldDef.name, fieldD);
            _fieldByFieldNumberMap.Add(fieldD.Number, fieldD);
        }

        for (int i = 0; i < def.nested_type.Count; i++)
        {
            var nestedDef = def.nested_type[i];

            var msgD = new Descriptor(pool, this, nestedDef);

            _nestedMsgMap.Add(nestedDef.name, msgD);
        }

        for (int i = 0; i < def.enum_type.Count; i++)
        {
            var nestedDef = def.enum_type[i];

            var enumD = new EnumDescriptor(pool, this, nestedDef);

            _nestedEnumMap.Add(nestedDef.name, enumD);
        }
    }

    public FieldDescriptor GetFieldByName( string name )
    {
        FieldDescriptor ret;
        if (_fieldByNameMap.TryGetValue(name, out ret))
        {
            return ret;
        }

        return null;
    }

    public FieldDescriptor GetFieldByFieldNumber(int fieldNumber )
    {
        FieldDescriptor ret;
        if (_fieldByFieldNumberMap.TryGetValue(fieldNumber, out ret))
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
        return Define.name;
    }

    public string Name
    {
        get { return Define.name; }
    }

}
