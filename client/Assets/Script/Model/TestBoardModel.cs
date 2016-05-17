using System;
using System.Collections.Generic;


class TestBoardModel
{
    string _TextInfo;

    gamedef.TestProfile _profile;

    gamedef.TestBag _bag;

    public Framework.ModelNotifier _notifier = new Framework.ModelNotifier();


    void SetProfile( gamedef.TestProfile value )
    {
        if ( value.HasHP )
        {
            _notifier.Notify("HP");
        }

        _profile = value;
    }

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
