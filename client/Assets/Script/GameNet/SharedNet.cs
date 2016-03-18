using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SharedNet
{

    static SharedNet _instance;
    static public SharedNet Instance
    {
        get {

            if ( _instance == null )
            {
                _instance = new SharedNet();
            }

            return _instance;
        }
    }

    public MessageMeta MsgMeta
    {
        get { return _meta; }
    }

    MessageMeta _meta = new MessageMeta();

    public SharedNet( )
    {
        _meta.Scan("gamedef");
    }
}
