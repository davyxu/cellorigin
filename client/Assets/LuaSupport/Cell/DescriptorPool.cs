using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using google.protobuf;


public class DescriptorPool
{
    Dictionary<string, Descriptor> _msgMap = new Dictionary<string, Descriptor>();
    public void Init( FileDescriptorSet fileset )
    {
        for (int i = 0; i < fileset.file.Count; i++)
        {
            var fileDef = fileset.file[i];

            var fileD = new FileDescriptor(null, fileDef);

            for (int f = 0; f < fileDef.message_type.Count; f++)
            {
                var def = fileDef.message_type[f];

                var msgD = new Descriptor(fileD, def);
                _msgMap.Add(msgD.FullName, msgD);
            }
        }
    }

    public Descriptor GetMessage( string name )
    {
        Descriptor ret;
        if ( _msgMap.TryGetValue( name, out ret ) )
        {
            return ret;
        }

        return null;
    }
}
