using System;
using System.Collections.Generic;



partial class LoginPresenter : BasePresenter
{
    LoginModel _Model;

    NetworkPeer _LoginPeer;
    NetworkPeer _GamePeer;


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

    public ObservableCollection<int, ServerInfoPresenter> ServerList { get; set; }

    void Init( )
    {
        _Model = ModelManager.Instance.Get<LoginModel>();

        ServerList = new ObservableCollection<int, ServerInfoPresenter>();

        // 通过切换代码生成器逻辑, 生成单元测试逻辑

        // Login Peer
        _LoginPeer = PeerManager.Instance.Get("login");

        _LoginPeer.RegisterMessage<gamedef.PeerConnected>(obj =>
        {
            Msg_LoginPeerConnected(_LoginPeer, obj as gamedef.PeerConnected);
        });

        _LoginPeer.RegisterMessage<gamedef.LoginACK>(obj =>
        {
            Msg_LoginACK(_LoginPeer, obj as gamedef.LoginACK);
        });


        // Game Peer
        _GamePeer = PeerManager.Instance.Get("game");

        _GamePeer.RegisterMessage<gamedef.PeerConnected>(obj =>
        {
            // 可以自定义回调名称避免冲突
            Msg_GamePeerConnected(_GamePeer, obj as gamedef.PeerConnected);
        });

       
        _GamePeer.RegisterMessage<gamedef.VerifyGameACK>(obj =>
        {
            Msg_VerifyGameACK(_GamePeer, obj as gamedef.VerifyGameACK);
        });
    }



}
