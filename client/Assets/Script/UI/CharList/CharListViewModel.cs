using System;
using System.Collections.Generic;


class CharListViewModel
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

    public ObservableList<SimpleCharInfoViewModel> CharInfoList { get; set; }

    #endregion



    NetworkPeer _gamePeer;

    public CharListViewModel( )
    {
        _gamePeer = PeerManager.Instance.Get("game");
        _Model = ModelManager.Instance.Get<CharListModel>();
    }

    public void Start( )
    {
        //Request();
    }

    public void Command_CreateChar( )
    {
        var req = new gamedef.CreateCharREQ();
        req.CharName = CharNameForCreate;
        req.CandidateID = 0;
        _gamePeer.SendMessage( req );
    }

    public void Command_SelectChar()
    {
        var req = new gamedef.EnterGameREQ();
        req.CharName = CharNameForEnter;
        _gamePeer.SendMessage(req);
    }

    void Test( )
    {
        
    }


    public void Request()
    {
        var req = new gamedef.CharListREQ();
        _gamePeer.SendMessage(req);

        _gamePeer.RegisterMessage<gamedef.CharListACK>(obj =>
        {
            var msg = obj as gamedef.CharListACK;

            _Model.CharInfo.FromList(msg.CharInfo);
        });
    }

}

