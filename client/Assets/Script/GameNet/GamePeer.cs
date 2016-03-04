using UnityEngine;
using Network;

class GamePeer : MonoBehaviour
{
    public string _Address;

    NetworkPeer _peer;    
    
    void Awake( )
    {
        _peer = new NetworkPeer("game", SharedNet.Instance.MsgMeta );

        _peer.RegisterEvent(NetworkEvent.Connected, msg =>
        {
            gamedef.EnterGameREQ req = new gamedef.EnterGameREQ();
            _peer.SendMsg(req);

        });

        _peer.RegisterMessage<gamedef.EnterGameACK>(msg =>
        {
            Debug.Log("EnterGameACK!");


        });
    }

    void Start( )
    {
        _peer.Start(_Address);
    }

    void OnDisable( )
    {
        if ( _peer != null )
        {
            _peer.Stop();
        }
    }

    void Update( )
    {
        if ( _peer != null )
        {
            _peer.Polling();
        }
    }
}
