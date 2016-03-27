using System;
using System.Collections.Generic;
using UnityEngine;
class LoginModel : BaseModel
{
    NetworkPeer _loginPeer;

    public string Account { get; set; }

    public string Address { get; set; }

    public List<gamedef.ServerInfo> ServerList;
    public Action OnLoginOK;


    public LoginModel( )
    {
        _loginPeer = PeerManager.Instance.Get("login");

        Load();
    }

    public void Load( )
    {
        var setting = LocalSetting.Load<gamedef.LoginSetting>("login");
        if ( setting != null )
        {
            Account = setting.Account;
            Address = setting.Address;
        }
    }

    public void Save( )
    {
        var setting = new gamedef.LoginSetting();
        setting.Account = Account;
        setting.Address = Address;
        LocalSetting.Save<gamedef.LoginSetting>("login", setting);
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
