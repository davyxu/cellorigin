using System;
using System.Collections.Generic;


public class LoginCharInfoModel
{
    public string CharName;
}

// 每个Item都有一个
partial class LoginCharInfoPresenter : Framework.BasePresenter
{
    public LoginCharInfoPresenter()
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

