using System.Collections.Generic;

partial class LoginView : BaseView
{
    void OnServerListChanged( )
    {
        _ServerList.ClearOptions();

        var nameList = new List<string>();
        for (int i = 0; i < _Presenter.ServerList.Count; i++)
        {
            nameList.Add(_Presenter.ServerList[i].DisplayName);

        }

        _ServerList.AddOptions(nameList);

        // 默认选中第一个
        if (_Presenter.ServerList.Count > 0)
        {
            _Presenter.CurrServerInfo = _Presenter.ServerList[_ServerList.value];
        }
    }


 
    void Awake( )
    {
        Bind( new LoginPresenter() );
    }

    void Start( )
    {
        _Presenter.Exec_Start();
    }

    void OnDisable()
    {
        _Presenter.Exec_SaveSetting();
    }
}
