using UnityEngine;
using System.Collections;

public class CellLuaClass : MonoBehaviour {


    public string ClassName;

    LuaInterface.LuaState _state;
   

   
    void Awake()
    {
        _state = CellLuaManager.GetMainState();
        if ( _state == null )
        {
            Debug.LogError("CellLuaLoader未挂载到场景中或优先度低于CellLuaClass");
            return;
        }



        CellLuaManager.Instance.ClassCallMethod(ClassName, "Awake", gameObject);
    }



	
	void Start () 
    {
        CellLuaManager.Instance.ClassCallMethod(ClassName, "Start", gameObject);
	}
}
