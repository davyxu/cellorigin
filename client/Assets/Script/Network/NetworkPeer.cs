using LuaInterface;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkPeer : MonoBehaviour
{
    public string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Address;

    public bool DebugMessage;

    ClientSocket _socket;
        
    MessageMetaSet _metaSet;    

    MessageDispatcher _dispatcher = new MessageDispatcher();
        
    public Action<NetworkPeer, uint, object> OnSend;
    public Action<NetworkPeer, uint, object> OnRecv;

    struct MsgData
    {
        public uint MsgID;
        public MemoryStream Data;

        public MsgData(uint msgid, MemoryStream data)
        {
            MsgID = msgid;
            Data = data;
        }
    }        

    Queue<MsgData> _msgQueue = new Queue<MsgData>();
    object _msgQueueGuard = new object();

    #region 延迟模拟

    // 延迟模拟系统
    struct DelayMsgData
    {
        public uint MsgID;
        public MemoryStream Data;
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

    #endregion

    uint MsgID_Connected;
    uint MsgID_Disconnected;
    uint MsgID_ConnectError;
    uint MsgID_SendError;
    uint MsgID_RecvError;

    public NetworkPeer( )
    {
        //DebugMessage = true;

        _metaSet = PeerManager.Instance.MsgMeta;

        MsgID_Connected = _metaSet.GetByType<gamedef.PeerConnected>().id;
        MsgID_Disconnected = _metaSet.GetByType<gamedef.PeerDisconnected>().id;
        MsgID_ConnectError = _metaSet.GetByType<gamedef.PeerConnectError>().id;
        MsgID_SendError = _metaSet.GetByType<gamedef.PeerSendError>().id;
        MsgID_RecvError = _metaSet.GetByType<gamedef.PeerRecvError>().id;
    }

   
    /// <summary>
    /// 连接是否可用
    /// </summary>
    public bool Valid
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
    public void Connect(string address)
    {
        // 防止重入

        if (_socket != null)
        {
            return;
        }

        _socket = new ClientSocket();

        _socket.OnRecv += (msgid, stream) =>
        {
            PostStream(msgid, stream);
        };

        _socket.OnConnected += delegate()
        {
            PostStream(MsgID_Connected, null);
        };

        _socket.OnDisconnected += delegate()
        {
            PostStream(MsgID_Disconnected, null);
        };

        _socket.OnError += delegate(NetworkReason reason )
        {
            switch( reason )
            {
                case NetworkReason.ConnectError:
                    {
                        PostStream(MsgID_ConnectError, null);
                    }
                    break;
                case NetworkReason.SendError:
                    {
                        PostStream(MsgID_SendError, null);
                    }
                    break;
                case NetworkReason.RecvError:
                    {
                        PostStream(MsgID_RecvError, null);
                    }
                    break;
            }
            
        };

        Address = address;
        _socket.Connect(address);

    }

    /// <summary>
    /// 手工投递一个消息
    /// </summary>
    /// <param name="msgID">消息ID</param>
    /// <param name="msg">消息内容</param>
    [NoToLuaAttribute]
    public void PostStream(uint msgID, MemoryStream stream)
    {
        lock (_msgQueueGuard)
        {
            MsgData md;
            md.Data = stream;
            md.MsgID = msgID;

            _msgQueue.Enqueue(md);
        }
    }

    [NoToLuaAttribute]
    public void Polling()
    {
        // 有延迟消息到达投递点
        if (_emulateDelayMS > 0 && _delayQueue.Count > 0)
        {
            var dm = _delayQueue.Peek();
            if (dm.IsTimeup(_emulateDelayMS))
            {
                _delayQueue.Dequeue();
                ProcessStream(dm.MsgID, dm.Data);
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
            ProcessStream(msg.MsgID, msg.Data);
        }
    }


    void ProcessStream(uint msgid, MemoryStream stream)
    {
        if (stream != null)
        {
            try
            {
                var meta = _metaSet.GetByID(msgid);
                if (meta != MessageMetaSet.NullMeta)
                {
                    var msg = ProtoBuf.Serializer.NonGeneric.Deserialize(meta.type, stream);

                    if (DebugMessage)
                    {
                        LogMessage(msgid, msg);
                    }

                    _dispatcher.Invoke(msgid, msg);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
       
        InvokeDecodeMessage(msgid, stream);
    }

    void LogMessage( uint msgid, object msg)
    {
        if (msgid == MsgID_Connected )
        {
            Debug.Log(string.Format("[{0}] Connected {1}", Name, Address));
        }
        else if ( msgid == MsgID_Disconnected )
        {
            Debug.Log(string.Format("[{0}] Disconnected {1}", Name, Address));
        }
        else if ( msgid == MsgID_ConnectError )
        {
            Debug.LogError(string.Format("[{0}] ConnectError {1}", Name, Address ));
        }
        else if (msgid == MsgID_RecvError)
        {
            Debug.LogError(string.Format("[{0}] RecvError", Name));
        }
        else if (msgid == MsgID_SendError)
        {
            Debug.LogError(string.Format("[{0}] SendError", Name));
        }
        else
        {
            Debug.Log(string.Format("[{0}] {1}|{2}", Name, msg.GetType().FullName, ProtobufText.Serializer.Serialize(msg)));
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

    #region C#接口

    /// <summary>
    /// 发一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="msg">消息内容</param>
    [NoToLuaAttribute]
    public void SendMessage<T>(T msg)
    {
        if (_socket == null)
            return;

        uint msgID = _metaSet.GetByType<T>().id;

        if (msgID == 0)
        {
            throw new InvalidCastException("Error when getting msgID:" + typeof(T).FullName);
        }

        if (DebugMessage)
        {
            LogMessage(msgID, msg);
        }


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
            Debug.LogError(e.ToString());
            return;
        }

        _socket.SendPacket(msgID, data.ToArray());

    }

    /// <summary>
    /// 手工投递一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="msg">消息内容</param>\
    [NoToLuaAttribute]
    public void PostMessage<T>(T msg)
    {
        var meta = _metaSet.GetByType<T>();

        if (meta == MessageMetaSet.NullMeta)
        {
            Debug.LogError("未注册的消息: " + typeof(T).FullName);
            return;
        }

        MemoryStream data = new MemoryStream();
        ProtoBuf.Serializer.Serialize(data, msg);

        PostStream(meta.id, data);
    }
    /// <summary>
    /// 注册一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="callback">回调处理</param>
    [NoToLuaAttribute]
    public void RegisterMessage<T>(Action<object> callback)
    {
        var meta = _metaSet.GetByType<T>();
        if (meta == MessageMetaSet.NullMeta)
        {
            Debug.LogError("未注册的消息:" + typeof(T).FullName);
            return;
        }

        _dispatcher.Add(meta.id, callback);
    }
    [NoToLuaAttribute]
    public void UnRegisterMessage<T>(Action<object> callback)
    {
        var meta = _metaSet.GetByType<T>();
        if (meta == MessageMetaSet.NullMeta)
        {
            Debug.LogError("未注册的消息:" + typeof(T).FullName);
            return;
        }

        _dispatcher.Remove(meta.id, callback);
    }

    #endregion

    #region Lua接口

    Dictionary<uint, Action<MemoryStream>> _scriptCallback = new Dictionary<uint,Action<MemoryStream>>();

    public void RegisterMessage( string msgName, LuaFunction func )
    {
        _scriptCallback.Add(StringUtility.HashNoCase(msgName), (stream) =>
        {
            CellLuaManager.NetworkDecodeRecv( this, msgName, stream, func);
        });
    }

    void InvokeDecodeMessage( uint msgid, MemoryStream stream )
    {
        Action<MemoryStream> callback;
        if ( _scriptCallback.TryGetValue( msgid, out callback ) )
        {
            callback(stream);
        }
    }
    public void SendMessage(string msgName, byte[] data)
    {
        if (_socket == null)
            return;

        var msgid = StringUtility.HashNoCase(msgName);
        
        _socket.SendPacket(msgid, data);
    }


    #endregion


}
   