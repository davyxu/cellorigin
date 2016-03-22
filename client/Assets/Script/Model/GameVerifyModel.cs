using System;
using System.Collections.Generic;
using UnityEngine;
class GameVerifyModel : Singleton<GameVerifyModel>
{
    NetworkPeer _gamePeer;

    public Action<gamedef.VerifyGameResult> OnVerifyResult;


    public GameVerifyModel()
    {
        _gamePeer = PeerManager.Instance.Get("game");
    }

    public void Request( string address, string token )
    {
        _gamePeer.Connect(address);

        _gamePeer.RegisterMessage<gamedef.PeerConnected>(obj =>
        {
            var req = new gamedef.VerifyGameREQ();
            req.Token = token;
            _gamePeer.SendMessage(req);
        });

        _gamePeer.RegisterMessage<gamedef.VerifyGameACK>(obj =>
        {
            var msg = obj as gamedef.VerifyGameACK;

            EventDispatcher.Instance.Invoke(msg.Result);
        });

    }
}
