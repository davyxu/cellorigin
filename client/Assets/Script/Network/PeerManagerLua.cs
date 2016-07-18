using UnityEngine;

public class PeerManagerLua : Singleton<PeerManagerLua>
{
    /// <summary>
    /// 在主摄像机上放置NetworkPeer
    /// </summary>
    /// <param name="name">提前命名</param>
    /// <returns></returns>
    public NetworkPeerLua Get(string name)
    {
        var cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("NetworkPeerLua 必须在主摄像机上");
            return null;
        }

        var peers = cam.GetComponents<NetworkPeerLua>();
        for (int i = 0; i < peers.Length; i++)
        {
            if (peers[i].Name == name)
                return peers[i];
        }

        // Make unity happy
        var com = cam.gameObject.AddComponent<NetworkPeerLua>();
        com.Name = name;
        return com;
    }
}

