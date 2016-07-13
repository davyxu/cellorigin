
using google.protobuf;

public class EnumValueDescriptor
{
    public EnumValueDescriptorProto Define { get; private set; }

    EnumDescriptor _d;

    [LuaInterface.NoToLuaAttribute]
    public EnumValueDescriptor(EnumDescriptor d, EnumValueDescriptorProto def)        
    {
        Define = def;        
        _d = d;
    }
   

    public int Number
    {
        get { return Define.number; }
    }

    public string Name
    {
        get { return Define.name; }
    }

}
