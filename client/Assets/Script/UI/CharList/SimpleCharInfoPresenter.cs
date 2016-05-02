using System;
using System.Collections.Generic;


public class SimpleCharInfoModel
{
    public string CharName;
}

// 每个Item都有一个
partial class SimpleCharInfoPresenter : BasePresenter
{
    public SimpleCharInfoPresenter()
    {
        Init();
    }


    public void Cmd_SelectChar( )
    {
        var peer = PeerManager.Instance.Get("game");

        var req = new gamedef.EnterGameREQ();
        req.CharName = CharName;
        peer.SendMessage(req);
    }



}

