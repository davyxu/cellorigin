using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ProtoBase
{
    ProtoBase _parent;    
    protected void Init( DescriptorPool pool, ProtoBase parent )
    {
        _parent = parent;
        Pool = pool;

        var nameList = new List<string>();

        nameList.Add(GetRawName());

        var p = this;
        while (p != null)
        {
            if (p._parent == null)
                break;

            p = p._parent;

            nameList.Add(p.GetRawName());
        }

        

        var sb = new StringBuilder();

        for (int i = nameList.Count - 1; i >= 0; i--)
        {
            sb.Append(nameList[i]);

            if (i >= 1)
            {
                sb.Append(".");
            }
        }

        FullName = sb.ToString();
    }

    protected virtual string GetRawName()
    {
        return default(string);
    }

    public string FullName { get; private set; }

    public DescriptorPool Pool { get; private set; }

}