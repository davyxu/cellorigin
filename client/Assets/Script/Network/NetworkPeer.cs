using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum NetworkEvent
{
    Connected = 1,
    Closed,
    MarshalError,
    UnmarshalError,
}

public class NetworkPeer : MonoBehaviour
{
    public string Name;
    public string Address;

    ClientSocket _socket;
        
    MessageMeta _meta;
    MessagePrinter _printer = new MessagePrinter();

    Dictionary<uint, Action<object>> _msgCallbacks = new Dictionary<uint, Action<object>>();
        
    public Action<NetworkPeer, uint, object> OnSend;
    public Action<NetworkPeer, uint, object> OnRecv;
    public Action<NetworkPeer> OnConnectionNotReady;

    struct MsgData
    {
        public uint MsgID;
        public object Data;

        public MsgData(uint msgid, object data)
        {
            MsgID = msgid;
            Data = data;
        }
    }        

    Queue<MsgData> _msgQueue = new Queue<MsgData>();
    object _msgQueueGuard = new object();
       

    // 延迟模拟系统
    struct DelayMsgData
    {
        public uint MsgID;
        public object Data;
        DateTime tick;

        public DelayMsgData(MsgData md)
        {
            MsgID = md.MsgID;
            Data = md.Data;
            tick = DateTime.UtcNow;
        }

        public bool IsTimeup(uint delayMs)
        {
            var dur = DateTime.UtcNow - tick;
            return (uint)dur.TotalMilliseconds > delayMs;
        }
    }

    // 模拟延迟使用的队列
    Queue<DelayMsgData> _delayQueue = new Queue<DelayMsgData>();

    uint _emulateDelayMS;
    public uint EmulateDelayMS
    {
        get { return _emulateDelayMS; }
        set { _emulateDelayMS = value; }
    }


    public NetworkPeer( )
    {
        _meta = SharedNet.Instance.MsgMeta;
    }

    /// <summary>
    /// 连接是否可用
    /// </summary>
    public bool Ready
    {
        get
        {

            if (_socket == null)
                return false;

            return _socket.IsConnected;
        }
    }

    /// <summary>
    /// 停止连接
    /// </summary>    
    public void Stop()
    {
        if (_socket != null)
        {
            _socket.Stop();

            _socket = null;
        }
    }

    /// <summary>
    /// 连接网络
    /// </summary>
    /// <param name="address">网络地址</param>
    public void Start(string address)
    {
        if (_socket != null)
        {
            return;
        }

        _socket = new ClientSocket();

        _socket.EventRecvPacket += OnReceiveSocketMessage;

        _socket.EventConnected += delegate()
        {                
            PostMessage((uint)NetworkEvent.Connected, null);
        };

        _socket.EventClosed += delegate(NetworkReason reason)
        {
            PostMessage((uint)NetworkEvent.Closed, reason);
        };


        // 将开始连接视为发送
        if (OnSend != null)
        {
            OnSend(this, (uint)NetworkEvent.Connected, null);
        }

        Address = address;
        _socket.Connect(address);


    }

                

    /// <summary>
    /// 发一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="msg">消息内容</param>
    public void SendMsg<T>(T msg)
    {
        if (_socket == null)
            return;

        if (!_socket.IsConnected)
        {
            if (OnConnectionNotReady != null)
            {
                OnConnectionNotReady.Invoke(this);
            }
        }

        uint msgID = _meta.GetMessageID<T>();

        if (msgID == 0)
        {
            throw new InvalidCastException("Error when getting msgID:" + typeof(T).FullName);
        }

        LogMessage(msgID, msg);

        if (OnSend != null)
        {
            OnSend(this, msgID, msg);
        }

        MemoryStream data = new MemoryStream();

        try
        {
            ProtoBuf.Serializer.Serialize(data, msg);                
        }
        catch (Exception e)
        {
            PostMessage((uint)NetworkEvent.MarshalError, e.ToString());
            return;
        }

        _socket.SendPacket(msgID, data);

    }        

    /// <summary>
    /// 手工投递一个消息
    /// </summary>
    /// <param name="msgID">消息ID</param>
    /// <param name="msg">消息内容</param>
    public void PostMessage(uint msgID, object msg)
    {
        lock (_msgQueueGuard)
        {
            MsgData md;
            md.Data = msg;
            md.MsgID = msgID;

            _msgQueue.Enqueue(md);
        }
    }

    /// <summary>
    /// 手工投递一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="msg">消息内容</param>
    public void PostMessage<T>(T msg)
    {
        uint msgID = _meta.GetMessageID<T>();

        if (msgID == 0)
        {
            throw new InvalidCastException("Error when getting msgID:" + typeof(T).FullName);
        }

        MemoryStream data = new MemoryStream();
        ProtoBuf.Serializer.Serialize(data, msg);

        PostMessage(msgID, data );
    }

    /// <summary>
    /// 注册一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="callback">回调处理</param>
    public void RegisterMessage<T>(Action<object> callback)
    {
        RegisterMessage(_meta.GetMessageID<T>(), callback);
    }

    /// <summary>
    /// 注册一个网络事件
    /// </summary>
    /// <param name="ev">事件类型</param>
    /// <param name="callback">回调处理</param>
    public void RegisterEvent(NetworkEvent ev, Action<object> callback)
    {
        RegisterMessage((uint)ev, callback);
    }


    public void UnRegisterEvent(NetworkEvent ev, Action<object> callback)
    {
        UnRegisterMessage((uint)ev, callback);
    }

    public void UnRegisterMessage(uint msgid, Action<object> callback)
    {
        Action<object> callbacks;
        if (_msgCallbacks.TryGetValue(msgid, out callbacks))
        {
            callbacks -= callback;
            _msgCallbacks[msgid] = callbacks; 
        }
    }
    public void UnRegisterMessage<T>(Action<object> callback)
    {
        UnRegisterMessage(_meta.GetMessageID<T>(), callback);
    }

    void RegisterMessage(uint msgid, Action<object> callback)
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


    void OnReceiveSocketMessage(uint msgid, MemoryStream stream)
    {
        object msg;

        try
        {
            var msgtype = _meta.GetMessageType(msgid);
            msg = ProtoBuf.Serializer.NonGeneric.Deserialize(msgtype, stream);
        }
        catch (Exception e)
        {
            PostMessage((uint)NetworkEvent.UnmarshalError, e.ToString());
            return;
        }

        if (msg == null)
        {
            return;
        }

        PostMessage(msgid, msg);
    }

    void DispatchMessage(uint msgid, object msg)
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


    public void Polling()
    {
        // 有延迟消息到达投递点
        if (_emulateDelayMS > 0 && _delayQueue.Count > 0)
        {
            var dm = _delayQueue.Peek();
            if (dm.IsTimeup(_emulateDelayMS))
            {
                _delayQueue.Dequeue();
                ProcessMessage(dm.MsgID, dm.Data);
            }
        }

        MsgData msg;

        lock (_msgQueueGuard)
        {
            if (_msgQueue.Count == 0)
            {
                return;
            }

            msg = _msgQueue.Dequeue();
        }

        if (_emulateDelayMS > 0)
        {
            _delayQueue.Enqueue(new DelayMsgData(msg));
        }
        else
        {
            ProcessMessage(msg.MsgID, msg.Data);
        }
    }


    void ProcessMessage(uint msgid, object msg)
    {
        LogMessage(msgid, msg);

        DispatchMessage(msgid, msg);
    }

    void LogMessage( uint msgid, object msg)
    {
        switch (msgid)
        {
            case (UInt32)NetworkEvent.Connected:
                Debug.Log(string.Format("[{0}] Connected", Name));
                break;
            case (UInt32)NetworkEvent.Closed:
                Debug.Log(string.Format("[{0}] Closed", Name));
                break;
            default:                    
                Debug.Log(string.Format("[{0}] {1}|{2}", Name, msg.GetType().FullName, _printer.Print(msg )));
                break;
        }

    }

    #region Unity stuff
    void Update( )
    {
        Polling();
    }

    void OnDisable()
    {
        Stop();
    }


    #endregion


}
   