using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkPeer : NetworkPeerBase
{

    MessageDispatcher _dispatcher = new MessageDispatcher();
    static Printer printer = new Printer();
    Sproto.SprotoPack spack = new Sproto.SprotoPack();

    protected MessageMetaSet _metaSet;

    public NetworkPeer()
    {
        _metaSet = PeerManager.Instance.MsgMeta;

        MsgID_Connected = _metaSet.GetByType<proto.PeerConnected>().id;
        MsgID_Disconnected = _metaSet.GetByType<proto.PeerDisconnected>().id;
        MsgID_ConnectError = _metaSet.GetByType<proto.PeerConnectError>().id;
        MsgID_SendError = _metaSet.GetByType<proto.PeerSendError>().id;
        MsgID_RecvError = _metaSet.GetByType<proto.PeerRecvError>().id;
    }

    HashSet<string> _group = new HashSet<string>();

    // 鼓励使用RegisterGroup+Add, 而不是Add+Remove
    public bool RegisterGroup(string name)
    {
        if (_group.Contains(name))
            return false;

        _group.Add(name);

        return true;
    }

    /// <summary>
    /// 发一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="msg">消息内容</param>

    public void SendMessage<T>(T msg) where T : Sproto.SprotoTypeBase
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
            ReflectMessage(msgID, msg);
        }
        

        try
        {
            var data = spack.pack(msg.encode());

            _socket.SendPacket(msgID, data);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return;
        }

        

    }
    /// <summary>
    /// 注册一个消息
    /// </summary>
    /// <typeparam name="T">消息类型</typeparam>
    /// <param name="callback">回调处理</param>

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

    Type[] tCache = new Type[] { typeof(byte[]) };
    object[] pCache = new object[1];

    protected override void ProcessStream(uint msgid, MemoryStream stream)
    {
        try
        {
            var meta = _metaSet.GetByID(msgid);
            if (meta != MessageMetaSet.NullMeta)
            {
                object msg = null;

                if (stream != null)
                {
                    pCache[0] = spack.unpack(stream.ToArray());
                    msg = meta.type.GetConstructor(tCache).Invoke(pCache);
                }
                
               
                if (DebugMessage)
                {
                    ReflectMessage(msgid, msg);
                }

                _dispatcher.Invoke(msgid, msg);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    void ReflectMessage(uint msgid, object msg)
    {
        if (msgid == MsgID_Connected)
        {
            Debug.Log(string.Format("[{0}] Connected {1}", Name, Address));
        }
        else if (msgid == MsgID_Disconnected)
        {
            Debug.Log(string.Format("[{0}] Disconnected {1}", Name, Address));
        }
        else if (msgid == MsgID_ConnectError)
        {
            Debug.LogError(string.Format("[{0}] ConnectError {1}", Name, Address));
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
            Debug.Log(string.Format("[{0}] {1}|{2}", Name, msg.GetType().FullName, printer.Print(msg)));
        }
    }
}
