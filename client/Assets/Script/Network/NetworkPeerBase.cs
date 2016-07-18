using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class NetworkPeerBase : MonoBehaviour
{
    public string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public string Address;

    public bool DebugMessage;

    #region 基本成员

    protected ClientSocket _socket;

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

    // 派生类赋值
    protected uint MsgID_Connected;
    protected uint MsgID_Disconnected;
    protected uint MsgID_ConnectError;
    protected uint MsgID_SendError;
    protected uint MsgID_RecvError;
    #endregion

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

    protected virtual void ProcessStream(uint msgid, MemoryStream stream)
    {

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

        _socket.OnError += delegate(NetworkReason reason)
        {
            switch (reason)
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

    #region Unity接口

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
   