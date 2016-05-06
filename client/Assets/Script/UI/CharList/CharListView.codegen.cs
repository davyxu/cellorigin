using UnityEngine.UI;

partial class CharListView : Framework.BaseView
{
    CharListPresenter _Presenter;

    InputField _CharName;
    Button _CreateChar;

    Framework.ListControl _CharList;

    Button _DebugAdd;
    Button _DebugRemove;
    Button _DebugModify;


    public override void Bind(Framework.BasePresenter presenter)
    {
        _Presenter = presenter as CharListPresenter;

        var trans = this.transform;
        _CharName = trans.Find("CharName").GetComponent<InputField>();
        _CreateChar = trans.Find("CreateChar").GetComponent<Button>();

        _CharList = trans.Find("CharList").GetComponent<Framework.ListControl>();


        _DebugAdd = trans.Find("DebugAdd").GetComponent<Button>();
        _DebugAdd.onClick.AddListener(_Presenter.Cmd_DebugAdd);
        _DebugRemove = trans.Find("DebugRemove").GetComponent<Button>();
        _DebugRemove.onClick.AddListener(_Presenter.Cmd_DebugRemove);
        _DebugModify = trans.Find("DebugModify").GetComponent<Button>();
        _DebugModify.onClick.AddListener(_Presenter.Cmd_DebugModify);


        _CreateChar.onClick.AddListener(_Presenter.Cmd_CreateChar);


        // CharList
        Framework.Utility.BindCollectionView<int, SimpleCharInfoPresenter, SimpleCharInfoView>(_Presenter.CharInfoList, _CharList);


        // CharName
        _CharName.onValueChanged.AddListener(x =>
        {
            _Presenter.CharNameForCreate = x;
        });
        // 初始化同步
        _Presenter.CharNameForCreate = _CharName.text;


    }
}
