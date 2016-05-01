using System;
using System.Collections.Generic;



partial class CharListPresenter : BasePresenter, ICharListPresenter
{
    public void Exec_CreateChar( )
    {
        var req = new gamedef.CreateCharREQ();
        req.CharName = CharNameForCreate;
        req.CandidateID = 0;
        _GamePeer.SendMessage( req );
    }

    public void Exec_SelectChar()
    {
        var req = new gamedef.EnterGameREQ();
        req.CharName = CharNameForEnter;
        _GamePeer.SendMessage(req);
    }

    static int _base = 1;
    public void Exec_DebugAdd( )
    {
        {
            var vm = new SimpleCharInfoPresenter();
            vm.CharName = _base.ToString();

            CharInfoList.Add(_base, vm);
            _base++;
        }

    }

    public void Exec_DebugRemove()
    {
        CharInfoList.Visit( (key, value ) =>{

            CharInfoList.Remove((int)key);


            return false;
        });
    }

    public void Exec_DebugModify( )
    {
        CharInfoList.Get(1).CharName = "m";
    }

    public CharListPresenter( )
    {
        Init();
    }

    private void Msg_CharListACK(NetworkPeer _GamePeer, gamedef.CharListACK msg)
    {
        CharInfoList.Clear();

        for (int i = 0; i < msg.CharInfo.Count; i++)
        {
            var sm = new SimpleCharInfoPresenter();
            sm.CharName = msg.CharInfo[i].CharName;
            CharInfoList.Add(i, sm);
        }
    }


    public void Cmd_Request()
    {
        var req = new gamedef.CharListREQ();
        _GamePeer.SendMessage(req);
    }

}

