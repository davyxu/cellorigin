using System.Collections.Generic;
using UnityEngine;

public partial class LoginUI : MonoBehaviour {

    LoginModel _model;

    void Awake( )
    {
        InitUI();

        _model = new LoginModel();

        _Account.text = _model.Account;
        _Address.text = _model.Address;

        _model.OnLoginOK += OnLoginOK;
        _EnterGame.interactable = false;

    }

    void OnDisable()
    {
        SaveSetting();

        _model.Save();
    }

    void Start( )
    {
        // 启动自动登陆
        _model.Login();
    }

    void OnLoginOK()
    {
        _EnterGame.interactable = true;

        // 刷新服务器列表
        RefreshServerList();
    }

    void EnterGame_Click()
    {
        // TODO 转圈的系统
        // TODO 连接有问题的提示, 区分连接不上和断开
        var svinfo = CurrServerInfo;
        if ( svinfo != null )
        {
            GameVerifyModel.Instance.Request(svinfo.Address, _model.Account);
        }

        // 进入游戏
        gameObject.SetActive(false);

        EventEnterGame ev;
        EventDispatcher.Instance.Invoke( ev );
    }




    void SetDevAddress_Click()
    {
        _Address.text = Constant.DevAddress;
    }

    void SetPublicAddress_Click()
    {
        _Address.text = Constant.PublicAddress;
    }

    /// <summary>
    /// 当前选中的服务器信息
    /// </summary>
    gamedef.ServerInfo CurrServerInfo
    {
        get
        {
            if ( _ServerList.value < 0 || _ServerList.value >= _model.ServerList.Count )
            {
                return null;
            }

            return _model.ServerList[_ServerList.value];
        }
    }

    
    void RefreshServerList( )
    {
        _ServerList.ClearOptions();

        var nameList = new List<string>();
        for( int i = 0;i< _model.ServerList.Count;i++)
        {
            nameList.Add(_model.ServerList[i].DisplayName);
            
        }
        
        _ServerList.AddOptions(nameList);

    }


    void SaveSetting()
    {
        _model.Account = _Account.text;
        _model.Address = _Address.text;
    }

}
