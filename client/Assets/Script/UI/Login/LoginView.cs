using System.Collections.Generic;

partial class LoginView : BaseView
{
    void Awake( )
    {
        Bind( new LoginPresenter() );
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
