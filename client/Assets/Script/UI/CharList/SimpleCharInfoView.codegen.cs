using UnityEngine.UI;



partial class SimpleCharInfoView : BaseView
{
    SimpleCharInfoPresenter _Presenter;

    Text _CharName;


    public SimpleCharInfoPresenter Presenter
    {
        get { return _Presenter; }
    }

    public override void Bind(BasePresenter vm)
    {
        _Presenter = vm as SimpleCharInfoPresenter;

        var trans = this.transform;
        _CharName = trans.Find("CharName").GetComponent<Text>();        
        

        // CharName
        _Presenter.OnCharNameChanged += delegate()
        {
            _CharName.text = _Presenter.CharName;
        };
        _CharName.text = _Presenter.CharName;


    }
}
