
using UnityEngine;

partial class LoginCharListView : Framework.BaseItemView
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
