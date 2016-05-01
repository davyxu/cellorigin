using System;

partial interface ILoginPresenter 
{
    #region Property
    string Account { get; set; }
    string Address { get; set; }

    #endregion

    #region Event
    event Action OnAccountChanged;

    event Action OnAddressChanged;
    #endregion

    #region Command
    void SetDevAddress();

    void SetPublicAddress();

    void EnterServer();

    void SaveSetting();

    void Start();

    #endregion
}
