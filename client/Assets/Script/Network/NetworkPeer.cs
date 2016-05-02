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
        
    MessageMeta _meta;
    MessagePrinter _printer = new MessagePrinter();

    MessageDispatcher _dispatcher = new MessageDispatcher();
        
    public Action<NetworkPeer, uint, object> OnSend;
    public Action<NetworkPeer, uint, object> OnRecv;

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

    #region 延迟模拟

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

    #endregion

    uint MsgID_Connected;
    uint MsgID_Disconnected;
    uint MsgID_ConnectError;
    uint MsgID_SendError;
    uint MsgID_RecvError;

    public NetworkPeer( )
    {
        DebugMessage = true;

        _meta = PeerManager.Instance.MsgMeta;

        MsgID_Connected = _meta.GetMessageID<gamedef.PeerConnected>();
        MsgID_Disconnected = _meta.GetMessageID<gamedef.PeerDisconnected>();
        MsgID_ConnectError = _meta.GetMessageID<gamedef.PeerConnectError>();
        MsgID_SendError = _meta.GetMessageID<gamedef.PeerSendError>();
        MsgID_RecvError = _meta.GetMessageID<gamedef.PeerRecvError>();
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

        _socket.OnRecv += OnReceiveSocketMessage;

        _socket.OnConnected += delegate()
        {
            PostMessage(MsgID_Connected, null);
        };

        _socket.OnDisconnected += delegate()
        {
            PostMessage(MsgID_Disconnected, null);
        };

        _socket.OnError += delegate(NetworkReason reason )
        {
            switch( reason )
            {
                case NetworkReason.ConnectError:
                    {
                        PostMessage(MsgID_ConnectError, null);
                    }
                    break;
                case NetworkReason.SendError:
                    {
                        PostMessage(MsgID_SendError, null);
                    }
                    break;
                case NetworkReason.RecvError:
                    {
                        PostMessage(MsgID_RecvError, null);
                    }
                    break;
            }
            
        };

        Address = address;
        _socket.Connect(address);

    }

                

    /// <summary>
    /// 发一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="msg">消息内容</param>
    public void SendMessage<T>(T msg)
    {
        if (_socket == null)
            return;

        uint msgID = _meta.GetMessageID<T>();

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
            Debug.LogError(e.ToString());
            return;
        }

        if (msg == null)
        {
            return;
        }

        PostMessage(msgid, msg);
    }

    /// <summary>
    /// 注册一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="callback">回调处理</param>
    public void RegisterMessage<T>(Action<object> callback)
    {
        _dispatcher.Add(_meta.GetMessageID<T>(), callback);
    }
    public void UnRegisterMessage<T>(Action<object> callback)
    {
        _dispatcher.Remove(_meta.GetMessageID<T>(), callback);
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
        if (DebugMessage)
        {
            LogMessage(msgid, msg);
        }
        

        _dispatcher.Invoke(msgid, msg);
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
            Debug.Log(string.Format("[{0}] ConnectError {1}", Name, Address ));
        }
        else if (msgid == MsgID_RecvError)
        {
            Debug.Log(string.Format("[{0}] RecvError", Name));
        }
        else if (msgid == MsgID_SendError)
        {
            Debug.Log(string.Format("[{0}] SendError", Name));
        }
        else
        {
            Debug.Log(string.Format("[{0}] {1}|{2}", Name, msg.GetType().FullName, _printer.Print(msg)));
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
   