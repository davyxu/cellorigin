using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoginUI : MonoBehaviour {

    public InputField Account;
    public InputField Address;
    public Button SetDevAddress;
    public Button SetPublicAddress;
    public Button GoLogin;

    gamedef.LoginSetting _setting;



    NetworkPeer _loginPeer;

    void Awake( )
    {
        _loginPeer = GetComponent<NetworkPeer>();

        _setting = LocalSetting.Load<gamedef.LoginSetting>("login");

        Account.text = _setting.Account;
        Address.text = _setting.Address;

        SetDevAddress.onClick.AddListener(() =>
        {
            Account.text = Constant.DevAddress;
        });

        SetPublicAddress.onClick.AddListener(() =>
        {
            Account.text = Constant.PublicAddress;
        });

        GoLogin.onClick.AddListener(() =>
        {
            // 登陆时, 保存设置
            SaveSetting();

            _loginPeer.Start(Address.text);
        });

    }
    

    void SaveSetting( )
    {
        _setting.Account = Account.text;
        _setting.Address = Address.text;
        LocalSetting.Save<gamedef.LoginSetting>("login", _setting );
    }


}
