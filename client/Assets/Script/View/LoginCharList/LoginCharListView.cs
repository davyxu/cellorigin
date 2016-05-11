
using UnityEngine;

partial class LoginCharListView : Framework.BaseView
{

    void Awake()
    {
        Bind(new LoginCharListPresenter());
    }

    void Start()
    {
        _Presenter.Cmd_Request();
    }
}
