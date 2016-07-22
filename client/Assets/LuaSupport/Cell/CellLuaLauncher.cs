using UnityEngine;

class CellLuaLauncher : MonoBehaviour
{
    void Awake( )
    {
        CellLuaManager.Attach();
    }

    void OnDestroy()
    {        
        CellLuaManager.Detach();
    }
}
