using System;
using System.Collections.Generic;


class LoginViewModel
{
    LoginModel _Model;

    #region Property

    public Action OnAccountChanged;
    public string Account
    {
        get
        {
            return _Model.Account;
        }

        set
        {
            _Model.Account = value;

            if (OnAccountChanged != null)
            {
                OnAccountChanged();
            }
        }
    }


    public Action OnAddressChanged;
    public string Address
    {
        get
        {
            return _Model.Address;
        }

        set
        {
            _Model.Address = value;

            if (OnAddressChanged != null)
            {
                OnAddressChanged();
            }
        }
    }

    public Action OnServerListChanged;
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

    gamedef.ServerInfo _currServerInfo;
    public gamedef.ServerInfo CurrServerInfo
    {
        get { return _currServerInfo; }
        set
        {
            _currServerInfo = value;
        }
    }


    #endregion

    #region Command
    public void Command_SetDevAddress()
    {
        Address = Constant.DevAddress;
    }

    public void Command_SetPublicAddress()
    {
        Address = Constant.PublicAddress;
    }

    public void Command_EnterServer( )
    {
        if ( CurrServerInfo != null )
        {
            VerifyGame(CurrServerInfo.Address, Account);
        }
        
    }

    public void Command_Login()
    {
        if (_loginPeer == null)
            return;

        _loginPeer.Connect(Address);

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

            // 停止线程及工作
            _loginPeer.Stop();
            _loginPeer = null;
        });

    }

    public void Command_SaveSetting( )
    {
        _Model.Save();
    }

    #endregion


    NetworkPeer _loginPeer;
    NetworkPeer _gamePeer;

    public LoginViewModel( )
    {
        _loginPeer = PeerManager.Instance.Get("login");
        _gamePeer = PeerManager.Instance.Get("game");
        _Model = ModelManager.Instance.Get<LoginModel>();
    }

    public void Start( )
    {
        _Model.Load();
        OnAccountChanged();
        OnAddressChanged();

        Command_Login();
    }

    public void VerifyGame(string address, string token)
    {
        // 处理重入
        if (_gamePeer.Valid)
            return;

        _gamePeer.Connect(address);

        _gamePeer.RegisterMessage<gamedef.PeerConnected>(obj =>
        {
            var req = new gamedef.VerifyGameREQ();
            req.Token = token;
            _gamePeer.SendMessage(req);
        });

        // TODO 通用对话框提示状态信息
        _gamePeer.RegisterMessage<gamedef.VerifyGameACK>(obj =>
        {
            var msg = obj as gamedef.VerifyGameACK;

            EventEmitter.Instance.Invoke(msg.Result);
        });

    }


}
