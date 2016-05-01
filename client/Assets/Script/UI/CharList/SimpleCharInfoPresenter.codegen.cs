using System;


interface ISimpleCharInfoPresenter
{
    string CharName { get; set; }

    event Action OnCharNameChanged;

    void Exec_SelectChar(object param);
}


partial class SimpleCharInfoPresenter : BasePresenter, ISimpleCharInfoPresenter
{
    SimpleCharInfoModel _Model;

    #region Property
    public event Action OnCharNameChanged;

    public string CharName
    {
        get
        {
            return _Model.CharName;
        }

        set
        {
            _Model.CharName = value;

            if (OnCharNameChanged != null)
            {
                OnCharNameChanged();
            }
        }
    }

    #endregion


    public void Init()
    {
        _Model = new SimpleCharInfoModel();
    }


}

