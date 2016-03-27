using System;
using System.Collections.Generic;
using UnityEngine;
class CharListModel : BaseModel
{
    NetworkPeer _gamePeer;

    public List<gamedef.SimpleCharInfo> CharInfo;


    public CharListModel( )
    {
        _gamePeer = PeerManager.Instance.Get("game");

        EventEmiiter.Instance.Add<gamedef.VerifyGameResult>(ev =>
        {
            if ( ev == gamedef.VerifyGameResult.VerifyOK )
            {
                Request();
            }
        });
    }

    public void Request( )
    {        
        var req = new gamedef.CharListREQ();
        _gamePeer.SendMessage(req);

        _gamePeer.RegisterMessage<gamedef.CharListACK>(obj =>
        {
            var msg = obj as gamedef.CharListACK;

            CharInfo = msg.CharInfo;

            if ( msg.CharInfo.Count == 0 )
            {
                
                EventEmiiter.Instance.Invoke<Event.CreateChar>();
                UIManager.Instance.Show("CreateCharUI");
            }
            else
            {
                EventEmiiter.Instance.Invoke<Event.ShowCharList>();
            }

        });
    }

    public void Create( int index, string name )
    {
        var req = new gamedef.CreateCharREQ();
        req.CandidateID = index;
        req.CharName = name;
        _gamePeer.SendMessage(req);
    }
}
