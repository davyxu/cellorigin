using System.Collections.Generic;

using google.protobuf;

public class Descriptor : ProtoBase
{
    public DescriptorProto Define { get; private set; }

    Dictionary<string, FieldDescriptor> _fieldByName = new Dictionary<string, FieldDescriptor>();
    Dictionary<int, FieldDescriptor> _fieldByFieldNumber = new Dictionary<int, FieldDescriptor>();

    Dictionary<string, Descriptor> _nestedMsg = new Dictionary<string, Descriptor>();

    Dictionary<string, EnumDescriptor> _nestedEnum = new Dictionary<string, EnumDescriptor>();

    [LuaInterface.NoToLuaAttribute]
    public Descriptor(DescriptorPool pool, ProtoBase parent, DescriptorProto def)        
    {
        Define = def;

        base.Init(pool, parent);

        // 字段
        for ( int i = 0;i< def.field.Count;i++)
        {
            var fieldDef = def.field[i];

            var fieldD = new FieldDescriptor( this, fieldDef );

            _fieldByName.Add(fieldDef.name, fieldD);
            _fieldByFieldNumber.Add(fieldD.Number, fieldD);
        }

        // 内嵌消息
        for (int i = 0; i < def.nested_type.Count; i++)
        {
            var nestedDef = def.nested_type[i];

            var msgD = new Descriptor(pool, this, nestedDef);

            _nestedMsg.Add(nestedDef.name, msgD);
        }

        // 内嵌枚举
        for (int i = 0; i < def.enum_type.Count; i++)
        {
            var nestedDef = def.enum_type[i];

            var enumD = new EnumDescriptor(pool, this, nestedDef);

            _nestedEnum.Add(nestedDef.name, enumD);
        }
    }

    public FieldDescriptor GetFieldByName( string name )
    {
        FieldDescriptor ret;
        if (_fieldByName.TryGetValue(name, out ret))
        {
            return ret;
        }

        return null;
    }

    public FieldDescriptor GetFieldByFieldNumber(int fieldNumber )
    {
        FieldDescriptor ret;
        if (_fieldByFieldNumber.TryGetValue(fieldNumber, out ret))
        {
            return ret;
        }

        return null;
    }

    public Descriptor GetNestedMessage(string name)
    {
        Descriptor ret;
        if (_nestedMsg.TryGetValue(name, out ret))
        {
            return ret;
        }

        return null;
    }

    public EnumDescriptor GetNestedEnum(string name)
    {
        EnumDescriptor ret;
        if (_nestedEnum.TryGetValue(name, out ret))
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
