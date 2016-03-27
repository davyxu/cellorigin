using System;
using System.Collections.Generic;
using UnityEngine;
class GameVerifyModel : BaseModel
{
    NetworkPeer _gamePeer;

    public Action<gamedef.VerifyGameResult> OnVerifyResult;


    public GameVerifyModel()
    {
        _gamePeer = PeerManager.Instance.Get("game");

        EventEmiiter.Instance.Add<Event.EnterServer>(ev =>
        {
            Request(ev.Address, ev.Token);
        });
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

        // TODO 通用对话框提示状态信息
        _gamePeer.RegisterMessage<gamedef.VerifyGameACK>(obj =>
        {
            var msg = obj as gamedef.VerifyGameACK;

            EventEmiiter.Instance.Invoke(msg.Result);
        });

    }
}
