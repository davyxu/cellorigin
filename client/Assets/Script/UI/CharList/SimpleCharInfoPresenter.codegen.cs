using System;


partial class SimpleCharInfoPresenter : Framework.BasePresenter
{
    SimpleCharInfoModel _Model;

    #region Property
    public Action OnCharNameChanged;

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

