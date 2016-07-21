using UnityEngine;
using System.Collections;

public class CellLuaClass : MonoBehaviour {

    [LuaInterface.NoToLuaAttribute]
    public string ClassName;

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


   
    void Awake()
    {
        // ClassName直接在Prefab中, 以后要去掉这种方式
        if ( !string.IsNullOrEmpty(ClassName) )
        {
            InitAwake(ClassName);
        }
    }

    public void InitAwake( string className )
    {
        ClassName = className;

        CellLuaManager.Attach();

        _hasMethod = CellLuaManager.ClassHasMethod(ClassName);

        CallMethod(MethodName.Awake);
    }

    void CallMethod( MethodName name )
    {
        if ( _hasMethod[(int)name] )
        {
            CellLuaManager.ClassCallMethod(ClassName, name.ToString(), gameObject);
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
