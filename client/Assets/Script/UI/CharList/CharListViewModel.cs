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

    public ObservableCollection<int, SimpleCharInfoViewModel> CharInfoList = new ObservableCollection<int, SimpleCharInfoViewModel>();

    #endregion



    NetworkPeer _gamePeer;

    public CharListViewModel( )
    {
        _gamePeer = PeerManager.Instance.Get("game");
        _Model = ModelManager.Instance.Get<CharListModel>();
    }

    public void Start( )
    {
        Request();
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

    public void Command_Add( )
    {
        {
            var vm = new SimpleCharInfoViewModel();
            vm.CharName = "a";

            CharInfoList.Add(1, vm);
        }

        {
            var vm = new SimpleCharInfoViewModel();
            vm.CharName = "b";

            CharInfoList.Add(2, vm);
        }

    }

    public void Command_Remove()
    {
        CharInfoList.Visit( (key, value ) =>{

            CharInfoList.Remove((int)key);


            return false;
        });
    }

    public void Command_Modify( )
    {
        CharInfoList.Get(1).CharName = "m";
    }


    public void Request()
    {
        var req = new gamedef.CharListREQ();
        _gamePeer.SendMessage(req);

        _gamePeer.RegisterMessage<gamedef.CharListACK>(obj =>
        {
            var msg = obj as gamedef.CharListACK;

            CharInfoList.Clear();

            for( int i = 0;i< msg.CharInfo.Count;i++)
            {
                var sm = new SimpleCharInfoViewModel();
                sm.CharName = msg.CharInfo[i].CharName;
                CharInfoList.Add(i, sm);
            }
        });
    }

}

