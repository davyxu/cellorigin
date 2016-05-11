
using UnityEngine;

partial class LoginCharBoardView : Framework.BaseView
{

    void Awake()
    {
        Bind(new LoginCharBoardPresenter());
    }

    void Start()
    {
        _Presenter.Cmd_Request();
    }
}
