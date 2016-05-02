using System;
using System.Collections.Generic;


partial class CharListPresenter : BasePresenter
{
    CharListModel _Model;

    #region Property
    public string CharNameForCreate
    {
        get
        {
            return _Model.CharNameForCreate;
        }

        set
        {
            _Model.CharNameForCreate = value;
        }
    }

    public string CharNameForEnter
    {
        get
        {
            return _Model.CharNameForEnter;
        }

        set
        {
            _Model.CharNameForEnter = value;
        }
    }

    public ObservableCollection<int, SimpleCharInfoPresenter> CharInfoList { get; set; }

    #endregion



    NetworkPeer _GamePeer;

    public void Init( )
    {
        // 属性初始化
        CharInfoList = new ObservableCollection<int, SimpleCharInfoPresenter>();

        _Model = ModelManager.Instance.Get<CharListModel>();

        _GamePeer = PeerManager.Instance.Get("game");

        _GamePeer.RegisterMessage<gamedef.CharListACK>(obj =>
        {
            Msg_CharListACK(_GamePeer, obj as gamedef.CharListACK);
        });

        
    }

}

