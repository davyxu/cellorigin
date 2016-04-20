using System;
using System.Collections.Generic;
using UnityEngine;
class LoginModel : BaseModel
{
    

    public string Account { get; set; }

    public string Address { get; set; }

    public List<gamedef.ServerInfo> ServerList;

    public void Load( )
    {
        var setting = LocalSetting.Load<gamedef.LoginSetting>("login");
        if ( setting != null )
        {
            Account = setting.Account;
            Address = setting.Address;
        }

        if ( string.IsNullOrEmpty(Account) )
        {
            Account = "t";
        }

        if (string.IsNullOrEmpty(Address))
        {
            Address = "127.0.0.1:8101";
        }
    }

    public void Save( )
    {
        var setting = new gamedef.LoginSetting();
        setting.Account = Account;
        setting.Address = Address;
        LocalSetting.Save<gamedef.LoginSetting>("login", setting);
    }


}
