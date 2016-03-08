using UnityEngine;
using Network;

class LoginPeer : MonoBehaviour
{
    public string _Address;

    NetworkPeer _peer;    
    
    void Awake( )
    {
        _peer = new NetworkPeer("login", SharedNet.Instance.MsgMeta );

        _peer.RegisterEvent(NetworkEvent.Connected, msg =>
        {
            var req = new gamedef.LoginREQ();
            _peer.SendMsg(req);

        });

        _peer.RegisterMessage<gamedef.LoginACK>(msg =>
        {
            var ack = msg as gamedef.LoginACK;

            GetComponent<GamePeer>().StartGame(ack.ServerList[0].Address);


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
