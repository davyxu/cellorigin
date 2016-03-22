using System;
using System.Collections.Generic;

class EventDataBase
{

}

class EventDataBaseTemplate<T> : EventDataBase
{
    public Action<T> callback;        
}



public class EventDispatcher : Singleton<EventDispatcher>
{
    Dictionary<Type, EventDataBase> _eventMap = new Dictionary<Type, EventDataBase>();

    public void Add<T>( Action<T> callback )
    {
        EventDataBaseTemplate<T> ev = new EventDataBaseTemplate<T>();
        ev.callback += callback;
        _eventMap.Add(typeof(T), ev);
    }

    public void Invoke<T>( T data )
    {
        EventDataBase ev;
        if ( _eventMap.TryGetValue(typeof(T), out ev ) )
        {
            var evi = ev as EventDataBaseTemplate<T>;
            evi.callback(data);
        }
    }
}
