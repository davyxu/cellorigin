using System;
using System.Collections.Generic;

partial class LoginView : Framework.BaseView
{
    void Awake( )
    {
        Bind(new LoginPresenter());
    }

    void Start( )
    {
        _Presenter.Cmd_Start();
    }

    void OnDisable()
    {
        _Presenter.Cmd_SaveSetting();
    }
}
