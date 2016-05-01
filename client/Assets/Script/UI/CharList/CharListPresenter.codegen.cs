using System;
using System.Collections.Generic;


interface ICharListPresenter
{
    #region Property
    string CharNameForCreate { get; set; }
    string CharNameForEnter { get; set; }

    ObservableCollection<int, SimpleCharInfoPresenter> CharInfoList { get; set; }

    #endregion

    #region Command
    void Exec_CreateChar();
    void Exec_SelectChar();

    void Exec_DebugAdd();

    void Exec_DebugRemove();

    void Exec_DebugModify();

    void Cmd_Request();

    #endregion
}



partial class CharListPresenter : BasePresenter, ICharListPresenter
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

