using LuaInterface;
using UnityEngine;


public class CellLuaManager : MonoBehaviour
{
    LuaState _state = null;
    LuaInterface.LuaFunction _callMethod;

    public static CellLuaManager Instance
    {
        get;
        protected set;
    }

    public static LuaState GetMainState()
    {
        if (Instance == null)
            return null;

        return Instance._state;
    }

    LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    void Awake( )
    {
        Instance = this;
        Init();
    }

    void Init( )
    {
        new LuaFileUtils();

        _state = new LuaState();

        _state.OpenLibs(LuaDLL.luaopen_pb);
        _state.OpenLibs(LuaDLL.luaopen_struct);
        
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        _state.OpenLibs(LuaDLL.luaopen_bit);
#endif
        _state.LuaSetTop(0);


        LuaBinder.Bind(_state);
        LuaCoroutine.Register(_state, this);

        

        _state.Start();        
        _state.AddSearchPath(Application.dataPath + "/LuaSupport/Cell");

        _state.DoFile("Main.lua");


        _callMethod = _state.GetFunction("Class.CallMethod");

        LuaFunction func = _state.GetFunction("Main");
        func.Call();
        func.Dispose();
        func = null;
    }

    public void ClassCallMethod(string className, string methodName, GameObject go )
    {
        if (_callMethod != null)
        {
            _callMethod.Call(className, methodName, go);
        }
    }

    protected void OnDestroy()
    {
        Destroy();
    }

    protected void OnApplicationQuit()
    {
        Destroy();
    }

    protected void Destroy()
    {
        if (_state != null)
        {
            _callMethod.Dispose();
            _callMethod = null;

            LuaState state = _state;
            _state = null;

            state.Dispose();
            Instance = null;
        }
    }


}
