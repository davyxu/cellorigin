using System;
using System.Collections.Generic;
using UnityEngine;
class LoginModel : BaseModel
{
    NetworkPeer _loginPeer;

    #region 登陆初始信息
    gamedef.LoginSetting _setting;
  
    public string Account
    {
        get { return _setting.Account; }
        set{ _setting.Account = value;}
    }

    public string Address
    {
        get { return _setting.Address; }
        set{ _setting.Address = value;}
    }
    #endregion

    public List<gamedef.ServerInfo> ServerList;
    public Action OnLoginOK;


    public LoginModel( )
    {
        _loginPeer = PeerManager.Instance.Get("login");

        _setting = LocalSetting.Load<gamedef.LoginSetting>("login");
    }

    public void Save( )
    {
        LocalSetting.Save<gamedef.LoginSetting>("login", _setting);
    }

    public void Login( )
    {
        _loginPeer.Connect( Address );

        _loginPeer.RegisterMessage<gamedef.PeerConnected>(obj =>
        {
            var req = new gamedef.LoginREQ();
            req.ClientVersion = Constant.ClientVersion;
            req.PlatformToken = Account;
            _loginPeer.SendMessage(req);
        });

        _loginPeer.RegisterMessage<gamedef.LoginACK>(obj =>
        {
            var msg = obj as gamedef.LoginACK;

            ServerList = msg.ServerList;

            OnLoginOK();

            // 停止线程及工作
            _loginPeer.Stop();
        });

    }
}
