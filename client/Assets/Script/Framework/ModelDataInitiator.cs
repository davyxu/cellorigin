using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ModelDataInitiator : MonoBehaviour
    {
        NetworkPeer _gamePeer;

        void Start( )
        {
            _gamePeer = PeerManager.Instance.Get("game");

        }
    }


}