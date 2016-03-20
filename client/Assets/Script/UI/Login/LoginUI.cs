using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public partial class LoginUI : MonoBehaviour {

    gamedef.LoginSetting _setting;

    NetworkPeer _loginPeer;

    void Awake( )
    {
        InitUI();

        _loginPeer = GetComponent<NetworkPeer>();

        _setting = LocalSetting.Load<gamedef.LoginSetting>("login");

        _Account.text = _setting.Account;
        _Address.text = _setting.Address;
    }

    void OnSetDevAddress( )
    {
        _Account.text = Constant.DevAddress;
    }

    void OnSetPublicAddress()
    {
        _Account.text = Constant.PublicAddress;
    }

    void OnLogin( )
    {
        // 登陆时, 保存设置
        SaveSetting();

        _loginPeer.Connect(_Address.text);
    }


    void SaveSetting()
    {
        _setting.Account = _Account.text;
        _setting.Address = _Address.text;
        LocalSetting.Save<gamedef.LoginSetting>("login", _setting);
    }

}
