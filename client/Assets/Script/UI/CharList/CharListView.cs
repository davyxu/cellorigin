
using UnityEngine;

partial class CharListView : Framework.BaseView
{

    void Awake()
    {
        Bind(new CharListPresenter());
    }

    void Start()
    {
        _Presenter.Cmd_Request();
    }
}
