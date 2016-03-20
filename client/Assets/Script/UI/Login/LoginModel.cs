using System;
using System.Collections.Generic;
using UnityEngine;
class LoginModel
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

    List<gamedef.ServerInfo> _serverList;
    public List<gamedef.ServerInfo> ServerList
    {
        get { return _serverList; }
    }

    public Action OnLoginOK;


    public LoginModel( GameObject loginui)
    {
        _loginPeer = loginui.GetComponent<NetworkPeer>();

        _setting = LocalSetting.Load<gamedef.LoginSetting>("login");
    }

    public void Login( )
    {
        LocalSetting.Save<gamedef.LoginSetting>("login", _setting);

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

            _serverList = msg.ServerList;

            OnLoginOK();
        });

    }
}
