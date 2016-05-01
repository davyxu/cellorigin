using System;
using System.Collections.Generic;


public class SimpleCharInfoModel
{
    public string CharName;
}


partial class SimpleCharInfoPresenter : BasePresenter, ISimpleCharInfoPresenter
{
    public SimpleCharInfoPresenter()
    {
        Init();
    }


    public void Exec_SelectChar( object param )
    {

    }

}

