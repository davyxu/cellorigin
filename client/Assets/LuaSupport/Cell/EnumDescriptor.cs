using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using google.protobuf;

public class EnumDescriptor : ProtoBase
{
    EnumDescriptorProto _def;

    Dictionary<string, EnumValueDescriptor> _fieldMap = new Dictionary<string, EnumValueDescriptor>();

    [LuaInterface.NoToLuaAttribute]
    public EnumDescriptor(DescriptorPool pool, ProtoBase parent, EnumDescriptorProto def)        
    {
        _def = def;

        base.Init(pool, parent);

        for ( int i = 0;i< _def.value.Count;i++)
        {
            var fieldDef = _def.value[i];

            var fieldD = new EnumValueDescriptor( this, fieldDef );

            _fieldMap.Add(fieldDef.name, fieldD);
        }
    }

    public EnumValueDescriptor GetValueByName(string name)
    {
        EnumValueDescriptor ret;
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
