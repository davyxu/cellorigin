using System;
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


    Dictionary<string, NetworkPeer> _peerMap = new Dictionary<string, NetworkPeer>();

    public void RegisterPeer(string name, NetworkPeer peer)
    {
        _peerMap.Add(name, peer);
    }

    public NetworkPeer Get( string name )
    {
        NetworkPeer peer;
        if ( _peerMap.TryGetValue( name, out peer ))
        {
            return peer;
        }

        return null;
    }
}

