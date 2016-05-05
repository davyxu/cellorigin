using UnityEngine.UI;

partial class LoginView : BaseView
{
    InputField _Account;
    InputField _Address;
    Button _SetDevAddress;
    Button _SetPublicAddress;
    ListControl _ServerList;

    LoginPresenter _Presenter;

    public override void Bind(BasePresenter presenter)
    {
        _Presenter = presenter as LoginPresenter;

        var trans = this.transform;
        _Account = trans.Find("Account").GetComponent<InputField>();
        _Address = trans.Find("Address").GetComponent<InputField>();
        _SetDevAddress = trans.Find("SetDevAddress").GetComponent<Button>();
        _SetPublicAddress = trans.Find("SetPublicAddress").GetComponent<Button>();
        _ServerList = trans.Find("ServerList").GetComponent<ListControl>();

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
        _Presenter.ServerList.OnItemTotalChanged += delegate
        {
            _ServerList.Clear<ServerInfoView>();

            _Presenter.ServerList.Visit((key, value) =>
            {
                _ServerList.Add<ServerInfoView, ServerInfoPresenter>(key, value);


                return true;
            });
        };

        _Presenter.ServerList.OnItemAdded += (key, value) =>
        {
            _ServerList.Add<ServerInfoView, ServerInfoPresenter>(key, value);
        };

        _Presenter.ServerList.OnItemRemoved += (key) =>
        {
            _ServerList.Remove<ServerInfoView>(key);
        };


    }
}
