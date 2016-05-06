using UnityEngine.UI;



partial class SimpleCharInfoView : Framework.BaseItemView
{
    SimpleCharInfoPresenter _Presenter;

    Text _CharName;
    Button _SelectChar;

    public override void Bind(Framework.BasePresenter presenter)
    {
        _Presenter = presenter as SimpleCharInfoPresenter;

        var trans = this.transform;
        _CharName = trans.Find("SelectChar/CharName").GetComponent<Text>();
        _SelectChar = trans.Find("SelectChar").GetComponent<Button>();

        // Item按钮传递
        _SelectChar.onClick.AddListener(_Presenter.Cmd_SelectChar);


        // CharName
        _Presenter.OnCharNameChanged += delegate()
        {
            _CharName.text = _Presenter.CharName;
        };
        _CharName.text = _Presenter.CharName;


    }
}
