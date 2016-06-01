using System;
using System.Collections.Generic;

public class MessageDispatcher
{
    Dictionary<uint, Action<object>> _msgCallbacks = new Dictionary<uint, Action<object>>();

    struct CallbackGuard
    {
        public uint msgid;
        public Action<object> callback;

        public override int GetHashCode()
        {
            return msgid.GetHashCode() + callback.GetHashCode();
        }
    }

    HashSet<CallbackGuard> _callbackGuard = new HashSet<CallbackGuard>();

    public void Add(uint msgid, Action<object> callback)
    {
        CallbackGuard guard;
        guard.msgid = msgid;
        guard.callback = callback;

        if (_callbackGuard.Contains(guard))
            return;

        _callbackGuard.Add(guard);

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
        CallbackGuard guard;
        guard.msgid = msgid;
        guard.callback = callback;

        _callbackGuard.Remove(guard);

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
