using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

class EventDataBase
{

}

class EventDataBaseTemplate<T> : EventDataBase
{
    public Action<T> callback;        
}



public class EventEmitter : Singleton<EventEmitter>
{
    Dictionary<Type, EventDataBase> _eventMap = new Dictionary<Type, EventDataBase>();

    /// <summary>
    /// 添加一个事件回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="callback"></param>
    public void Add<T>( Action<T> callback )
    {
        EventDataBaseTemplate<T> ev = new EventDataBaseTemplate<T>();
        ev.callback += callback;
        _eventMap.Add(typeof(T), ev);
    }

    /// <summary>
    /// 调用事件回调
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    public void Invoke<T>( T data = default(T))
    {
        var info = TypeToString(data);
        Debug.Log("EventInvoke: " + info);

        EventDataBase ev;
        if ( _eventMap.TryGetValue(typeof(T), out ev ) )
        {
            var evi = ev as EventDataBaseTemplate<T>;
            evi.callback(data);
        }
    }


    string TypeToString<T>( T instance )
    {
        var sb = new StringBuilder();

        var info = typeof(T);

        sb.Append(info.Name);
        sb.Append("{ ");

        var fields = info.GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach( FieldInfo f in fields )
        {
            sb.Append(f.Name);
            sb.Append(": ");

            var v = f.GetValue(instance).ToString();
            sb.Append(v);
            sb.Append(" ");
        }

        sb.Append("}");

        return sb.ToString();
    }
}
