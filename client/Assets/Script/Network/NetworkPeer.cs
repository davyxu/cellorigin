using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetworkPeer : NetworkPeerBase
{

    MessageDispatcher _dispatcher = new MessageDispatcher();

    protected MessageMetaSet _metaSet;

    public NetworkPeer()
    {
        _metaSet = PeerManager.Instance.MsgMeta;

        MsgID_Connected = _metaSet.GetByType<gamedef.PeerConnected>().id;
        MsgID_Disconnected = _metaSet.GetByType<gamedef.PeerDisconnected>().id;
        MsgID_ConnectError = _metaSet.GetByType<gamedef.PeerConnectError>().id;
        MsgID_SendError = _metaSet.GetByType<gamedef.PeerSendError>().id;
        MsgID_RecvError = _metaSet.GetByType<gamedef.PeerRecvError>().id;
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
            ReflectMessage(msgID, msg);
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

    protected override void ProcessStream(uint msgid, MemoryStream stream)
    {
        try
        {
            var meta = _metaSet.GetByID(msgid);
            if (meta != MessageMetaSet.NullMeta)
            {
                var msg = ProtoBuf.Serializer.NonGeneric.Deserialize(meta.type, stream);

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
            Debug.Log(string.Format("[{0}] {1}|{2}", Name, msg.GetType().FullName, ProtobufText.Serializer.Serialize(msg)));
        }
    }
}
