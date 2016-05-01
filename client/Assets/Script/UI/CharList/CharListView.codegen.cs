using UnityEngine.UI;

partial class CharListView : BaseView
{
    ICharListPresenter _Presenter;

    InputField _CharName;
    Button _CreateChar;

    ListControl _CharList;

    Button _DebugAdd;
    Button _DebugRemove;
    Button _DebugModify;


    public override void Bind( BasePresenter presenter)
    {
        _Presenter = presenter as ICharListPresenter;

        var trans = this.transform;
        _CharName = trans.Find("CharName").GetComponent<InputField>();
        _CreateChar = trans.Find("CreateChar").GetComponent<Button>();

        _CharList = trans.Find("CharList").GetComponent<ListControl>();


        _DebugAdd = trans.Find("DebugAdd").GetComponent<Button>();
        _DebugAdd.onClick.AddListener(_Presenter.Exec_DebugAdd);
        _DebugRemove = trans.Find("DebugRemove").GetComponent<Button>();
        _DebugRemove.onClick.AddListener(_Presenter.Exec_DebugRemove);
        _DebugModify = trans.Find("DebugModify").GetComponent<Button>();
        _DebugModify.onClick.AddListener(_Presenter.Exec_DebugModify);


        _CreateChar.onClick.AddListener(_Presenter.Exec_CreateChar);


        // CharList
        _Presenter.CharInfoList.OnItemTotalChanged += delegate {
            _CharList.Clear<SimpleCharInfoView>();

            _Presenter.CharInfoList.Visit((key, value) =>
            {
                _CharList.Add<SimpleCharInfoView, SimpleCharInfoPresenter>(key, value);
                

                return true;
            });
        };

        _Presenter.CharInfoList.OnItemAdded += (key, value) =>
        {
            _CharList.Add<SimpleCharInfoView, SimpleCharInfoPresenter>(key, value);
        };

        _Presenter.CharInfoList.OnItemRemoved += ( key ) =>
        {
            _CharList.Remove<SimpleCharInfoView>(key);
        };


        // CharName
        _CharName.onValueChanged.AddListener(x =>
        {
            _Presenter.CharNameForCreate = x;
        });
        // 初始化同步
        _Presenter.CharNameForCreate = _CharName.text;


    }
}
