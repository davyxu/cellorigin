using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class LoginView : MonoBehaviour
{
    InputField _Account;
    InputField _Address;
    Button _SetDevAddress;
    Button _SetPublicAddress;
    Dropdown _ServerList;
    Button _EnterServer;

    LoginViewModel _ViewModel;

    void InitUI( )
    {
        _ViewModel = new LoginViewModel();

        var trans = this.transform;
        _Account = trans.Find("Account").GetComponent<InputField>();
        _Address = trans.Find("Address").GetComponent<InputField>();
        _SetDevAddress = trans.Find("SetDevAddress").GetComponent<Button>();
        _SetPublicAddress = trans.Find("SetPublicAddress").GetComponent<Button>();
        _ServerList = trans.Find("ServerList").GetComponent<Dropdown>();
        _EnterServer = trans.Find("EnterServer").GetComponent<Button>();

        _SetDevAddress.onClick.AddListener(_ViewModel.Command_SetDevAddress);
        _SetPublicAddress.onClick.AddListener(_ViewModel.Command_SetPublicAddress);
        _EnterServer.onClick.AddListener(delegate(){

            // 隐藏自己
            gameObject.SetActive(false);

            _ViewModel.Command_EnterServer();
        
        });


        // Account
        _Account.onValueChanged.AddListener(x =>
        {
            _ViewModel.Account = x;
        });

        _ViewModel.OnAccountChanged += delegate()
        {
            _Account.text = _ViewModel.Account;
        };

        // Address
        _Address.onValueChanged.AddListener(x =>
        {
            _ViewModel.Address = x;
        });
        _ViewModel.OnAddressChanged += delegate()
        {
            _Address.text = _ViewModel.Address;
        };

        // ServerList
        _ViewModel.OnServerListChanged += delegate()
        {
            _ServerList.ClearOptions();

            var nameList = new List<string>();
            for (int i = 0; i < _ViewModel.ServerList.Count; i++)
            {
                nameList.Add(_ViewModel.ServerList[i].DisplayName);

            }

            _ServerList.AddOptions(nameList);

            // 默认选中第一个
            if ( _ViewModel.ServerList.Count > 0 )
            {
                _ViewModel.CurrServerInfo = _ViewModel.ServerList[_ServerList.value];
            }
            

        };
    }


 
    void Awake( )
    {
        InitUI();
    }

    void Start( )
    {
        _ViewModel.Start();
    }

    void OnDisable()
    {
        _ViewModel.Command_SaveSetting();
    }
}
