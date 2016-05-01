using System;
using System.Collections.Generic;


public class SimpleCharInfoModel
{
    public string CharName;
}


class SimpleCharInfoPresenter : BasePresenter
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


    public SimpleCharInfoPresenter()
    {
        _Model = new SimpleCharInfoModel();
    }


}

