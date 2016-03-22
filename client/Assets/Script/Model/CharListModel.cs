using System;
using System.Collections.Generic;
using UnityEngine;
class CharListModel
{
    NetworkPeer _gamePeer;

    public Action OnCharListOK;

    public List<gamedef.SimpleCharInfo> CharInfo;


    public CharListModel( )
    {
        _gamePeer = PeerManager.Instance.Get("game");
    }

    public void Request( )
    {        
        var req = new gamedef.CharListREQ();
        _gamePeer.SendMessage(req);

        _gamePeer.RegisterMessage<gamedef.CharListACK>(obj =>
        {
            var msg = obj as gamedef.CharListACK;

            CharInfo = msg.CharInfo;

            OnCharListOK();
        });

    }
}
