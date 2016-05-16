using System.IO;
using UnityEngine;
using UnityEngine.UI;



partial class TestBoardView : Framework.BaseView
{
    public void Start( )
    {
        var a = new gamedef.LoginSetting();
        a.Account = "hello";

        var writer = new System.IO.MemoryStream();

        ProtoBuf.Serializer.Serialize(writer, a);

        writer.Position = 0;
        var c = ProtoBuf.Serializer.Deserialize<gamedef.LoginSetting>(writer);
    }


	
}
