using System;
using System.Collections.Generic;
using System.Reflection;


public class MessageMeta
{
    Dictionary<uint, Type> _id2type = new Dictionary<uint, Type>();
    Dictionary<Type, uint> _type2id = new Dictionary<Type, uint>();

    /// <summary>
    /// 扫描一个命名空间下的所有消息, 并以名字注册为消息
    /// </summary>
    /// <param name="NameSpace">命名空间名字</param>
    /// <returns></returns>
    public MessageMeta Scan( string NameSpace )
    {
        foreach( Type t in Assembly.GetExecutingAssembly().GetTypes() )
        {
            if ( t.Namespace == NameSpace && t.IsClass )
            {
                RegisterMessage(Utility.StringUtility.HashNoCase(t.FullName), t);
            }
        }

        return this;
    }

    void RegisterMessage( uint id, Type t )
    {
        if (GetMessageType(id) == t)
        {
            throw new Exception("Duplicate message id");
        }

        _id2type.Add(id, t);
        _type2id.Add(t, id);
    }

    /// <summary>
    /// 根据ID取到消息的类型
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Type GetMessageType( uint id )
    {
        Type t;
        if (_id2type.TryGetValue(id, out t))
        {
            return t;
        }

        return null;
    }

    /// <summary>
    /// 根据类型取到ID
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public uint GetMessageID( Type t )
    {
        uint id;
        if (_type2id.TryGetValue(t, out id))
        {
            return id;
        }

        return 0;
    }

    public uint GetMessageID<T>( )
    {
        return GetMessageID(typeof(T));
    }
}
