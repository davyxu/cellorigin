
using google.protobuf;

public class FileDescriptor : ProtoBase
{
    public FileDescriptorProto Define { get; private set; }

    [LuaInterface.NoToLuaAttribute]
    public FileDescriptor(DescriptorPool pool, ProtoBase parent, FileDescriptorProto def)        
    {
        Define = def;

        base.Init(pool, parent);
    }

    protected override string GetRawName()
    {
        return Define.package;
    }

    public string Name
    {
        get { return Define.name; }
    }

}
