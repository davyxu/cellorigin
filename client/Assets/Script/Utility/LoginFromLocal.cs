using UnityEngine;

[RequireComponent(typeof(NetworkPeer))]
class LoginFromLocal : MonoBehaviour
{    

    void Start( )
    {        
        var setting = LocalSetting.Load<gamedef.LoginSetting>("login");
        var loginPeer = GetComponent<NetworkPeer>();

        
        loginPeer.Address = setting.Address;
        
    }
}
