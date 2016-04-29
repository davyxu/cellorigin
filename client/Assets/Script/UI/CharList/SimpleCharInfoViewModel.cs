using System;
using System.Collections.Generic;


public class SimpleCharInfoModel
{
    public string CharName;
}


class SimpleCharInfoViewModel
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


    public SimpleCharInfoViewModel()
    {
        _Model = new SimpleCharInfoModel();
    }

    public void Start( )
    {
        //Request();
    }


}

