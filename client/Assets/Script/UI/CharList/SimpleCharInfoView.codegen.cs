using UnityEngine.UI;



partial class SimpleCharInfoView : BaseItemView
{
    ISimpleCharInfoPresenter _Presenter;

    Text _CharName;
    Button _SelectChar;

    public override void Bind(BasePresenter vm)
    {
        _Presenter = vm as ISimpleCharInfoPresenter;

        var trans = this.transform;
        _CharName = trans.Find("SelectChar/CharName").GetComponent<Text>();
        _SelectChar = trans.Find("SelectChar").GetComponent<Button>();

        _SelectChar.onClick.AddListener(delegate()
        {
            _Presenter.Exec_SelectChar(1);

        });

        // CharName
        _Presenter.OnCharNameChanged += delegate()
        {
            _CharName.text = _Presenter.CharName;
        };
        _CharName.text = _Presenter.CharName;


    }
}
