using UnityEngine.UI;

partial class CharListView : BaseView
{
    CharListPresenter _Presenter;

    InputField _CharName;
    Button _SelectChar;
    Button _CreateChar;

    ListControl _CharList;

    Button _Add;
    Button _Remove;
    Button _Modify;


    public override void Bind( BasePresenter presenter)
    {
        _Presenter = presenter as CharListPresenter;

        var trans = this.transform;
        _CharName = trans.Find("CharName").GetComponent<InputField>();
        _SelectChar = trans.Find("SelectChar").GetComponent<Button>();
        _CreateChar = trans.Find("CreateChar").GetComponent<Button>();

        _CharList = trans.Find("CharList").GetComponent<ListControl>();


        _Add = trans.Find("Add").GetComponent<Button>();
        _Add.onClick.AddListener(_Presenter.Command_Add);
        _Remove = trans.Find("Remove").GetComponent<Button>();
        _Remove.onClick.AddListener(_Presenter.Command_Remove);
        _Modify = trans.Find("Modify").GetComponent<Button>();
        _Modify.onClick.AddListener(_Presenter.Command_Modify);


        _CreateChar.onClick.AddListener(_Presenter.Command_CreateChar);
        _SelectChar.onClick.AddListener(_Presenter.Command_SelectChar);


        // CharList
        _Presenter.CharInfoList.OnItemTotalChanged += delegate {
            _CharList.Clear();

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
            _CharList.Remove(key);
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
