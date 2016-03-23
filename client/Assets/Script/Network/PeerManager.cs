using System;
using UnityEngine;
using System.Collections.Generic;

public class PeerManager : Singleton<PeerManager>
{
    MessageMeta _meta;

    public MessageMeta MsgMeta
    {
        get { return _meta; }
    }

    public PeerManager( )
    {
        _meta = new MessageMeta();
        _meta.Scan("gamedef");
    }

    /// <summary>
    /// 在主摄像机上放置NetworkPeer
    /// </summary>
    /// <param name="name">提前命名</param>
    /// <returns></returns>
    public NetworkPeer Get( string name )
    {
        var cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("NetworkPeer 必须在主摄像机上");
            return null;
        }

        var peers = cam.GetComponents<NetworkPeer>();
        for( int i = 0;i< peers.Length;i++)
        {
            if (peers[i].Name == name)
                return peers[i];
        }

        return null;
    }
}

