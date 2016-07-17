using UnityEngine;
using System.Collections;

public class CellLuaClass : MonoBehaviour {


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
