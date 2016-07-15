using System.Collections.Generic;

using google.protobuf;

public class EnumDescriptor : ProtoBase
{
    public EnumDescriptorProto Define { get; private set; }

    Dictionary<string, EnumValueDescriptor> _fieldByName = new Dictionary<string, EnumValueDescriptor>();
    Dictionary<int, EnumValueDescriptor> _fieldByNumber = new Dictionary<int, EnumValueDescriptor>();

    [LuaInterface.NoToLuaAttribute]
    public EnumDescriptor(DescriptorPool pool, ProtoBase parent, EnumDescriptorProto def)        
    {
        Define = def;

        base.Init(pool, parent);

        for ( int i = 0;i< def.value.Count;i++)
        {
            var fieldDef = def.value[i];

            var fieldD = new EnumValueDescriptor( this, fieldDef );

            _fieldByName.Add(fieldDef.name, fieldD);
            _fieldByNumber.Add(fieldDef.number, fieldD);
        }
    }

    public EnumValueDescriptor GetValueByName(string name)
    {
        EnumValueDescriptor ret;
        if (_fieldByName.TryGetValue(name, out ret))
        {
            return ret;
        }

        return null;
    }

    public EnumValueDescriptor GetValueByNumber(int number)
    {
        EnumValueDescriptor ret;
        if (_fieldByNumber.TryGetValue(number, out ret))
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
