using UnityEngine;
using System.Collections;

public class CellLuaClass : MonoBehaviour {

    [LuaInterface.NoToLuaAttribute]
    public string LuaClassName;

    public enum MethodName
    {
        Awake = 0,
        OnEnable,
        Start,
        OnDisable,
        OnDestroy,
        Max,
    }

    bool[] _hasMethod = new bool[(int)MethodName.Max];


    public void InitAwake( string className )
    {
        LuaClassName = className;

        CellLuaManager.Attach();

        _hasMethod = CellLuaManager.ClassHasMethod(LuaClassName);

        CallMethod(MethodName.Awake);
    }

    void CallMethod( MethodName name )
    {
        if ( _hasMethod[(int)name] )
        {
            CellLuaManager.ClassCallMethod(LuaClassName, name.ToString(), gameObject);
        }
    }

    void OnEnable( )
    {
        CallMethod(MethodName.OnEnable);
    }
    void OnDisable()
    {
        CallMethod(MethodName.OnDisable);
    }

    void OnDestroy( )
    {
        CallMethod(MethodName.OnDestroy);

        CellLuaManager.Detach();
    }

	
	void Start () 
    {
        CallMethod(MethodName.Start);
	}
}
