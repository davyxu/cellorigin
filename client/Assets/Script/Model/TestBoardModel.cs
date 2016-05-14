using System;
using System.Collections.Generic;


class TestBoardModel
{
    public string _TextInfo;



    public Action OnTextInfoChanged;
    public string TextInfo
    {
        get
        {
            return _TextInfo;
        }
        set
        {
            _TextInfo = value;

            if (OnTextInfoChanged != null)
            {
                OnTextInfoChanged();
            }
        }
    }

}
