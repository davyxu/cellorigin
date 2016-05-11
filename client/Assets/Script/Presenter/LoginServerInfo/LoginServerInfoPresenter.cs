using System;
using System.Collections.Generic;


// 每个Item都有一个
partial class LoginServerInfoPresenter : Framework.BasePresenter
{
    gamedef.ServerInfo _Model;

    #region Property

    public Action OnNameChanged;
    public string Name
    {
        get
        {
            // 这里是自定义显示, 所以代码生成无法做
            return string.Format("{0} {1}", _Model.DisplayName, _Model.Address);
        }
    }

    #endregion


    // model采用传入方式绑定
    public LoginServerInfoPresenter(gamedef.ServerInfo model)
    {
        _Model = model;
    }


    public void Cmd_Select( )
    {
        var peer = PeerManager.Instance.Get("game");

        // 处理重入
        if (peer.Valid)
            return;

        Framework.ViewManager.Instance.Hide("Login");

        peer.Connect(_Model.Address);
    }



}

