using System;
using System.Collections.Generic;
using UnityEngine;


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
            Event.EnterServer ev;
            ev.Address = CurrServerInfo.Address;
            ev.Token = Account;
            EventEmiiter.Instance.Invoke(ev);
        }
        
    }

    public void Command_Login()
    {
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

            //OnLoginOK();

            // 停止线程及工作
            _loginPeer.Stop();
        });

    }

    public void Command_SaveSetting( )
    {
        _Model.Save();
    }

    #endregion


    NetworkPeer _loginPeer;

    public LoginViewModel( )
    {
        _loginPeer = PeerManager.Instance.Get("login");
        _Model = ModelManager.Instance.Get<LoginModel>();
    }

    public void Start( )
    {
        _Model.Load();
        OnAccountChanged();
        OnAddressChanged();

        Command_Login();
    }


}
