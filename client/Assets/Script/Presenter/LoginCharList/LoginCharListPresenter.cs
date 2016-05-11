using System;
using System.Collections.Generic;



partial class LoginCharListPresenter : Framework.BasePresenter
{
    public void Cmd_CreateChar( )
    {
        var req = new gamedef.CreateCharREQ();
        req.CharName = CharName;
        req.CandidateID = 0;
        _gamePeer.SendMessage( req );
    }


    static int _base = 1;
    public void Cmd_DebugAdd( )
    {
        {
            var vm = new LoginCharInfoPresenter();
            vm.CharName = _base.ToString();

            LoginCharInfoCollection.Add(_base, vm);
            _base++;
        }

    }

    public void Cmd_DebugRemove()
    {
        LoginCharInfoCollection.Visit((key, value) =>
        {

            LoginCharInfoCollection.Remove((int)key);


            return false;
        });
    }

    public void Cmd_DebugModify( )
    {
        LoginCharInfoCollection.Get(1).CharName = "m";
    }

    public LoginCharListPresenter()
    {
        Init();
    }

    public void Msg_game_CharListACK(NetworkPeer peer, gamedef.CharListACK msg)
    {
        LoginCharInfoCollection.Clear();

        for (int i = 0; i < msg.CharInfo.Count; i++)
        {
            var sm = new LoginCharInfoPresenter();
            sm.CharName = msg.CharInfo[i].CharName;
            LoginCharInfoCollection.Add(i, sm);
        }

    }


    public void Cmd_Request()
    {
        var req = new gamedef.CharListREQ();
        _gamePeer.SendMessage(req);
    }

}

