using System;
using System.Collections.Generic;


class CharListViewModel
{
    CharListModel _Model;
    NetworkPeer _gamePeer;

    public CharListViewModel( )
    {
        _gamePeer = PeerManager.Instance.Get("game");
        _Model = ModelManager.Instance.Get<CharListModel>();

        EventEmitter.Instance.Add<gamedef.VerifyGameResult>(ev =>
        {
            if (ev == gamedef.VerifyGameResult.VerifyOK)
            {
                Request();
            }
        });
    }

    public void Request()
    {
        var req = new gamedef.CharListREQ();
        _gamePeer.SendMessage(req);

        _gamePeer.RegisterMessage<gamedef.CharListACK>(obj =>
        {
            var msg = obj as gamedef.CharListACK;

            _Model.CharInfo = msg.CharInfo;

            if (msg.CharInfo.Count == 0)
            {

                EventEmitter.Instance.Invoke<Event.CreateChar>();
                UIManager.Instance.Show("CreateCharUI");
            }
            else
            {
                EventEmitter.Instance.Invoke<Event.ShowCharList>();
            }

        });
    }

}

