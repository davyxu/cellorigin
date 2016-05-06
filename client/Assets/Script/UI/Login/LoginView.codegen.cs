using UnityEngine.UI;

partial class LoginView : Framework.BaseView
{
    InputField _Account;
    InputField _Address;
    Button _SetDevAddress;
    Button _SetPublicAddress;
    Framework.ListControl _ServerList;

    LoginPresenter _Presenter;

    public override void Bind(Framework.BasePresenter presenter)
    {
        _Presenter = presenter as LoginPresenter;

        var trans = this.transform;
        _Account = trans.Find("Account").GetComponent<InputField>();
        _Address = trans.Find("Address").GetComponent<InputField>();
        _SetDevAddress = trans.Find("SetDevAddress").GetComponent<Button>();
        _SetPublicAddress = trans.Find("SetPublicAddress").GetComponent<Button>();
        _ServerList = trans.Find("ServerList").GetComponent<Framework.ListControl>();

        _SetDevAddress.onClick.AddListener(_Presenter.Cmd_SetDevAddress);
        _SetPublicAddress.onClick.AddListener(_Presenter.Cmd_SetPublicAddress);        


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
        Framework.Utility.BindCollectionView<int, ServerInfoPresenter, ServerInfoView>(_Presenter.ServerList, _ServerList);
    }


}
