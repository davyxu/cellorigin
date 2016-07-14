using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using google.protobuf;


public class DescriptorPool
{
    Dictionary<string, Descriptor> _msgByName = new Dictionary<string, Descriptor>();

    Dictionary<string, EnumDescriptor> _enumByName = new Dictionary<string, EnumDescriptor>();

    [LuaInterface.NoToLuaAttribute]
    public void Init( FileDescriptorSet fileset )
    {
        for (int i = 0; i < fileset.file.Count; i++)
        {
            var fileDef = fileset.file[i];

            var fileD = new FileDescriptor(this, null, fileDef);


            // 全局消息
            for (int f = 0; f < fileDef.message_type.Count; f++)
            {
                var def = fileDef.message_type[f];

                var msgD = new Descriptor(this, fileD, def);
                _msgByName.Add(msgD.FullName, msgD);
            }

            // 全局枚举
            for (int f = 0; f < fileDef.enum_type.Count; f++)
            {
                var def = fileDef.enum_type[f];

                var enumD = new EnumDescriptor(this, fileD, def);
                _enumByName.Add(enumD.FullName, enumD);
            }
        }
    }

    public Descriptor GetMessage( string name )
    {
        Descriptor ret;
        if ( _msgByName.TryGetValue( name, out ret ) )
        {
            return ret;
        }

        return null;
    }

    public EnumDescriptor GetEnum(string name)
    {
        EnumDescriptor ret;
        if (_enumByName.TryGetValue(name, out ret))
        {
            return ret;
        }

        return null;
    }
}
