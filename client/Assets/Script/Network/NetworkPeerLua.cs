using LuaInterface;
using System.IO;

public class NetworkPeerLua : NetworkPeerBase
{    
    MessageDispatcher _dispatcher = new MessageDispatcher();

    public NetworkPeerLua( )
    {
        MsgID_Connected = StringUtility.Hash("gamedef.PeerConnected");
        MsgID_Disconnected = StringUtility.Hash("gamedef.PeerDisconnected");
        MsgID_ConnectError = StringUtility.Hash("gamedef.PeerConnectError");
        MsgID_SendError = StringUtility.Hash("gamedef.PeerSendError");
        MsgID_RecvError = StringUtility.Hash("gamedef.PeerRecvError");
    }

    public void RegisterMessage(string msgName, LuaFunction func)
    {
        _dispatcher.Add(StringUtility.Hash(msgName), (obj) =>
        {
            CellLuaManager.NetworkDecodeRecv(this, msgName, obj as MemoryStream, func);
        });
    }

    public void SendMessage(string msgName, PBStreamWriter writer)
    {
        if (_socket == null)
            return;

        var msgid = StringUtility.Hash(msgName);

        _socket.SendPacket(msgid, writer.ToArray());
    }

    protected override void ProcessStream(uint msgid, MemoryStream stream)
    {
        _dispatcher.Invoke(msgid, stream);
    }
}
