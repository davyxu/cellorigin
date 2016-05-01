using System;
using System.Collections.Generic;

partial interface ILoginPresenter
{

    event Action OnServerListChanged;
    List<gamedef.ServerInfo> ServerList { get; set; }
    gamedef.ServerInfo CurrServerInfo { get; set; }
}


partial class LoginPresenter : BasePresenter, ILoginPresenter
{

    #region Property

    public event Action OnServerListChanged;

    // 带有pb类型的非标准结构, 且一般一次性刷新的, 可以处理为手工列表
    public List<gamedef.ServerInfo> ServerList
    {
        get { return _Model.ServerList; }
        set 
        { 
            _Model.ServerList = value;

            if (OnServerListChanged != null )
            {
                OnServerListChanged();
            }
        }
    }

    public gamedef.ServerInfo CurrServerInfo{get;set;}


    #endregion

    #region Command
    public void Exec_SetDevAddress()
    {
        Address = Constant.DevAddress;
    }

    public void Exec_SetPublicAddress()
    {
        Address = Constant.PublicAddress;
    }

    public void Exec_EnterServer( )
    {
        UIManager.Instance.Hide("LoginUI");

        if ( CurrServerInfo != null )
        {
            VerifyGame(CurrServerInfo.Address, Account);
        }
        
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

    public void Exec_Start( )
    {
        LoadSetting();

        StartLogin();
    }

    public void Exec_SaveSetting()
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
        ServerList = msg.ServerList;

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
    public void VerifyGame(string address, string token)
    {
        // 处理重入
        if (_GamePeer.Valid)
            return;

        _GamePeer.Connect(address);

        _verifyToken = token;
    }


    void Msg_VerifyGameACK(NetworkPeer peer, gamedef.VerifyGameACK msg)
    {
        // TODO 通用对话框提示状态信息

        switch (msg.Result)
        {
            case gamedef.VerifyGameResult.VerifyOK:
                {
                    UIManager.Instance.Show("CharListUI");
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
