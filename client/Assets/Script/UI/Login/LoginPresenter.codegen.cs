using System;
using System.Collections.Generic;


partial interface ILoginPresenter
{
    #region Property
    string Account { get; set; }
    string Address { get; set; }

    #endregion

    #region Event
    event Action OnAccountChanged;

    event Action OnAddressChanged;
    #endregion

    #region Command
    void Exec_SetDevAddress();

    void Exec_SetPublicAddress();

    void Exec_EnterServer();

    void Exec_SaveSetting();

    void Exec_Start();

    #endregion
}



partial class LoginPresenter : BasePresenter, ILoginPresenter
{
    LoginModel _Model;

    NetworkPeer _LoginPeer;
    NetworkPeer _GamePeer;


    public event Action OnAccountChanged;
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


    public event Action OnAddressChanged;
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

    void Init( )
    {
        _Model = ModelManager.Instance.Get<LoginModel>();

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
