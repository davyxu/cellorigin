using UnityEngine.UI;

partial class LoginView : BaseView
{
    InputField _Account;
    InputField _Address;
    Button _SetDevAddress;
    Button _SetPublicAddress;
    Dropdown _ServerList;
    Button _EnterServer;

    ILoginPresenter _Presenter;

    public override void Bind(BasePresenter presenter)
    {
        _Presenter = presenter as ILoginPresenter;

        var trans = this.transform;
        _Account = trans.Find("Account").GetComponent<InputField>();
        _Address = trans.Find("Address").GetComponent<InputField>();
        _SetDevAddress = trans.Find("SetDevAddress").GetComponent<Button>();
        _SetPublicAddress = trans.Find("SetPublicAddress").GetComponent<Button>();
        _ServerList = trans.Find("ServerList").GetComponent<Dropdown>();
        _EnterServer = trans.Find("EnterServer").GetComponent<Button>();

        _SetDevAddress.onClick.AddListener(_Presenter.Exec_SetDevAddress);
        _SetPublicAddress.onClick.AddListener(_Presenter.Exec_SetPublicAddress);
        _EnterServer.onClick.AddListener(_Presenter.Exec_EnterServer);


        // Account
        _Account.onValueChanged.AddListener(x =>
        {
            _Presenter.Account = x;
        });

        _Presenter.OnAccountChanged += delegate()
        {
            _Account.text = _Presenter.Account;
        };

        // Address
        _Address.onValueChanged.AddListener(x =>
        {
            _Presenter.Address = x;
        });
        _Presenter.OnAddressChanged += delegate()
        {
            _Address.text = _Presenter.Address;
        };

        // ServerList
        _Presenter.OnServerListChanged += OnServerListChanged;
    }
}
