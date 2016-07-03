using UnityEngine;
using System.Collections;

public class CellLuaClass : MonoBehaviour {


    public string ClassName;

    LuaInterface.LuaState _state;
    LuaInterface.LuaFunction _callMethod;

    enum MethodName
    {
        Awake = 0,
        OnEnable,
        Start,
        Update,
        OnDisable,
        OnDestroy,
        Max,
    }

    bool[] _hasMethod = new bool[(int)MethodName.Max];
    

    void Awake()
    {
        _state = CellLuaLoader.GetMainState();        

        _callMethod = _state.GetFunction("Class.CallMethod");

        CallMethod(MethodName.Awake);
    }

    // 返回脚本实际存在的函数情况来决定是否调用其函数
    void MakeHasMethod( )
    {
        var hasMethod = _state.GetFunction("Class.HasMethod");
        if (hasMethod != null)
        {
            var ret = hasMethod.Call(ClassName)[0] as LuaInterface.LuaTable;

            if (ret.Length != _hasMethod.Length)
            {
                Debug.LogError("Class.lua的函数存在数量与CellLuaClass不相符");
            }
            else
            {
                for (int i = 0; i < ret.Length; i++)
                {
                    _hasMethod[i] = (bool)ret[i + 1];
                }
            }

            hasMethod.Dispose();
            hasMethod = null;
        }
    }

    void CallMethod(MethodName name )
    {
        if (!_hasMethod[(int)name])
        {
            return;
        }

        if (_callMethod != null)
        {
            _callMethod.Call(ClassName, name.ToString(), gameObject);
        }
    }
	
	void Start () 
    {
        CallMethod(MethodName.Start);
	
	}
	
	void Update () 
    {        
        CallMethod(MethodName.Update);
	}

    void OnDestroy( )
    {
        if (_callMethod != null)
        {
            CallMethod(MethodName.OnDestroy);

            _callMethod.Dispose();
            _callMethod = null;
        }
    }
}
