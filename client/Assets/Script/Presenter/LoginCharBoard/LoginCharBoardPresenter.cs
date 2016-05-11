using System;
using System.Collections.Generic;



partial class LoginCharBoardPresenter : Framework.BasePresenter
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

            LoginCharList.Add(_base, vm);
            _base++;
        }

    }

    public void Cmd_DebugRemove()
    {
        LoginCharList.Visit((key, value) =>
        {

            LoginCharList.Remove((int)key);


            return false;
        });
    }

    public void Cmd_DebugModify( )
    {
        LoginCharList.Get(1).CharName = "m";
    }

    public LoginCharBoardPresenter()
    {
        Init();
    }

    public void Msg_game_CharListACK(NetworkPeer peer, gamedef.CharListACK msg)
    {
        LoginCharList.Clear();

        for (int i = 0; i < msg.CharInfo.Count; i++)
        {
            var sm = new LoginCharInfoPresenter();
            sm.CharName = msg.CharInfo[i].CharName;
            LoginCharList.Add(i, sm);
        }

    }


    public void Cmd_Request()
    {
        var req = new gamedef.CharListREQ();
        _gamePeer.SendMessage(req);
    }

}

