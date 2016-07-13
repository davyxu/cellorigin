using LuaInterface;
using System.IO;
using UnityEngine;


public class CellLuaManager
{
    static LuaState _state = null;
    static LuaInterface.LuaFunction _callMethod;
    static LuaInterface.LuaFunction _hasMethod;
    static LuaInterface.LuaFunction _decodeRecvMethod;


    static int _ref;

    public static void Attach( )
    {
        if ( _ref == 0 )
        {
            Init();
        }

        _ref++;
    }

    public static void Detach()
    {
        if (_state == null)
            return;

        _ref--;

        if ( _ref == 0 )
        {
            Destroy();
        }
    }

    static void Destroy()
    {

        _decodeRecvMethod.Dispose();
        _decodeRecvMethod = null;

        _hasMethod.Dispose();
        _hasMethod = null;

        _callMethod.Dispose();
        _callMethod = null;

        LuaState state = _state;
        _state = null;

        state.Dispose();
        state = null;
    }


    static void Init()
    {
        new LuaFileUtils();

        _state = new LuaState();

        _state.OpenLibs(LuaDLL.luaopen_protobuf_c);
        _state.OpenLibs(LuaDLL.luaopen_struct);
        
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        _state.OpenLibs(LuaDLL.luaopen_bit);
#endif
        _state.LuaSetTop(0);

        LuaPB.GetTestData();

        LuaBinder.Bind(_state);

        _state.Start();        
        _state.AddSearchPath(Application.dataPath + "/LuaSupport/Cell");

        _state.DoFile("Main.lua");


        _callMethod = _state.GetFunction("Class.CallMethod");
        _hasMethod = _state.GetFunction("Class.HasMethod");
        _decodeRecvMethod = _state.GetFunction("Network.DecodeRecv");

        LuaFunction func = _state.GetFunction("Main");
        func.Call();
        func.Dispose();
        func = null;
    }


    public static void NetworkDecodeRecv( NetworkPeer peer, string msgName, MemoryStream stream, LuaFunction func )
    {
        if (_decodeRecvMethod != null)
        {
            if ( stream == null )
            {
                _decodeRecvMethod.Call(peer, msgName, null, func);
            }
            else
            {
                _decodeRecvMethod.Call(peer, msgName, new LuaByteBuffer(stream.ToArray()), func);            
            }
            
        }
    }

       
    public static void ClassCallMethod(string className, string methodName, GameObject go )
    {
        if (_callMethod != null)
        {
            _callMethod.Call(className, methodName, go);
        }
    }


    // 返回脚本实际存在的函数情况来决定是否调用其函数
    public static bool[] ClassHasMethod(string className)
    {
        var methodExistsStatus = new bool[(int)CellLuaClass.MethodName.Max];

        if (_hasMethod != null)
        {
            var ret = _hasMethod.Call(className)[0] as LuaInterface.LuaTable;

            for (int i = 0; i < ret.Length; i++)
            {
                methodExistsStatus[i] = (bool)ret[i + 1];
            }            
        }

        return methodExistsStatus;
    }




}
