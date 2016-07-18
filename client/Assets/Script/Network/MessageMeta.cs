using System;
using System.Collections.Generic;
using System.Reflection;

public class MessageMeta
{
    public Type type;
    public uint id;

    public string name
    {
        get { return type.FullName; }
    }

    public MessageMeta(Type t)
    {
        this.type = t;
        this.id = StringUtility.Hash(t.FullName);
    }
}



/// <summary>
/// 消息类型与id的映射表
/// </summary>
public class MessageMetaSet
{
    
    public readonly static MessageMeta NullMeta = new MessageMeta(typeof(MessageMeta));

    Dictionary<uint, MessageMeta> _idmap = new Dictionary<uint, MessageMeta>();
    Dictionary<Type, MessageMeta> _typemap = new Dictionary<Type, MessageMeta>();
    Dictionary<string, MessageMeta> _namemap = new Dictionary<string, MessageMeta>();

    /// <summary>
    /// 扫描一个命名空间下的所有消息, 并以名字注册为消息
    /// </summary>
    /// <param name="NameSpace">命名空间名字</param>
    /// <returns></returns>
    public MessageMetaSet Scan( string NameSpace )
    {
        foreach( Type t in Assembly.GetExecutingAssembly().GetTypes() )
        {
            if ( t.Namespace == NameSpace && t.IsClass )
            {
                RegisterMessage(t);
            }
        }

        return this;
    }

    /// <summary>
    /// 将消息注册
    /// </summary>
    /// <param name="id"></param>
    /// <param name="t"></param>
    void RegisterMessage( Type t )
    {
        if (GetByType(t) != NullMeta)
        {
            throw new Exception("重复的消息ID");
        }

        var mi = new MessageMeta(t);
        mi.id = StringUtility.Hash(t.FullName);
        mi.type = t;

        _idmap.Add(mi.id, mi);
        _typemap.Add(t, mi);
        _namemap.Add(mi.name, mi);
    }

    /// <summary>
    /// 根据ID取到消息的类型
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MessageMeta GetByID( uint id )
    {
        MessageMeta t;
        if (_idmap.TryGetValue(id, out t))
        {
            return t;
        }

        return NullMeta;
    }

    /// <summary>
    /// 根据类型取到ID
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public MessageMeta GetByType(Type t)
    {
        MessageMeta mi;
        if (_typemap.TryGetValue(t, out mi))
        {
            return mi;
        }

        return NullMeta;
    }

    public MessageMeta GetByName(string name)
    {
        MessageMeta mi;
        if (_namemap.TryGetValue(name, out mi))
        {
            return mi;
        }

        return NullMeta;
    }

    public MessageMeta GetByType<T>()
    {
        return GetByType(typeof(T));
    }


}
