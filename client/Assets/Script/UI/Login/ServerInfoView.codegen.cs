using UnityEngine.UI;



partial class ServerInfoView : BaseItemView
{
    ServerInfoPresenter _Presenter;

    Text _Name;
    Button _Select;

    public override void Bind(BasePresenter presenter)
    {
        _Presenter = presenter as ServerInfoPresenter;

        var trans = this.transform;
        _Name = trans.Find("Select/Name").GetComponent<Text>();
        _Select = trans.Find("Select").GetComponent<Button>();

        // Item按钮传递
        _Select.onClick.AddListener(_Presenter.Cmd_Select);


        // CharName
        _Name.text = _Presenter.Name;


    }
}
