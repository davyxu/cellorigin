
partial class CharListView : BaseView
{
    void Awake()
    {
        Bind(new CharListPresenter());
    }

    void Start()
    {
        _Presenter.Start();
    }
}
