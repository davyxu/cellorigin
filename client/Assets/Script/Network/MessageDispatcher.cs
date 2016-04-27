using System;
using System.Collections.Generic;

public class MessageDispatcher
{
    Dictionary<uint, Action<object>> _msgCallbacks = new Dictionary<uint, Action<object>>();

    public void Add(uint msgid, Action<object> callback)
    {
        Action<object> callbacks;
        if (_msgCallbacks.TryGetValue(msgid, out callbacks))
        {
            callbacks += callback;
            _msgCallbacks[msgid] = callbacks;
        }
        else
        {
            callbacks += callback;

            _msgCallbacks.Add(msgid, callbacks);
        }
    }

    public void Remove(uint msgid, Action<object> callback)
    {
        Action<object> callbacks;
        if (_msgCallbacks.TryGetValue(msgid, out callbacks))
        {
            callbacks -= callback;
            _msgCallbacks[msgid] = callbacks;
        }
    }

    public void Invoke(uint msgid, object msg)
    {
        Action<object> callbacks;
        if (!_msgCallbacks.TryGetValue(msgid, out callbacks))
        {
            return;
        }

        if (callbacks != null)
        {
            callbacks.Invoke(msg);
        }
        else
        {
            _msgCallbacks.Remove(msgid);
        }
    }
}
