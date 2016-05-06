using System;
using System.Collections.Generic;

partial class LoginPresenter : Framework.BasePresenter
{

    #region Property


    //public gamedef.ServerInfo CurrServerInfo{get;set;}


    #endregion

    #region Command
    public void Cmd_SetDevAddress()
    {
        Address = Constant.DevAddress;
    }

    public void Cmd_SetPublicAddress()
    {
        Address = Constant.PublicAddress;
    }

    public void StartLogin()
    {
        if (_LoginPeer == null || _LoginPeer.Valid)
            return;

        _LoginPeer.Connect(Address);
    }


    #endregion




    public LoginPresenter( )
    {
        Init();
    }

    public void Cmd_Start( )
    {
        LoadSetting();

        StartLogin();
    }

    public void Cmd_SaveSetting()
    {
        SaveSetting();
    }


    #region Setting
    void LoadSetting( )
    {
        var setting = LocalSetting.Load<gamedef.LoginSetting>("login");
        if (setting != null)
        {
            Account = setting.Account;
            Address = setting.Address;
        }

        if (string.IsNullOrEmpty(Account))
        {
            Account = "t";
        }

        if (string.IsNullOrEmpty(Address))
        {
            Address = "127.0.0.1:8101";
        }
    }

    void SaveSetting( )
    {
        var setting = new gamedef.LoginSetting();
        setting.Account = Account;
        setting.Address = Address;
        LocalSetting.Save<gamedef.LoginSetting>("login", setting);
    }
    #endregion

    #region Login Message

    void Msg_LoginPeerConnected(NetworkPeer peer, gamedef.PeerConnected msg)
    {
        var req = new gamedef.LoginREQ();
        req.ClientVersion = Constant.ClientVersion;
        req.PlatformToken = Account;
        _LoginPeer.SendMessage(req);
    }

    void Msg_LoginACK(NetworkPeer peer, gamedef.LoginACK msg)
    {
        ServerList.Clear();

        for( int i = 0;i< msg.ServerList.Count;i++)
        {
            ServerList.Add(i, new ServerInfoPresenter( msg.ServerList[i] ));
        }

        _verifyToken = msg.Token;

        // 停止线程及工作
        _LoginPeer.Stop();
        _LoginPeer = null;
    }

    #endregion

    #region Game Message

    string _verifyToken;
    void Msg_GamePeerConnected(NetworkPeer peer, gamedef.PeerConnected msg)
    {
        var req = new gamedef.VerifyGameREQ();
        req.Token = _verifyToken;
        peer.SendMessage(req);
    }



    void Msg_VerifyGameACK(NetworkPeer peer, gamedef.VerifyGameACK msg)
    {
        // TODO 通用对话框提示状态信息

        switch (msg.Result)
        {
            case gamedef.VerifyGameResult.VerifyOK:
                {
                    Framework.ViewManager.Instance.Show("CharListUI");
                }
                break;
            default:
                {
                    // TODO 通用对话框处理
                }
                break;
        }
    }

    #endregion



}
