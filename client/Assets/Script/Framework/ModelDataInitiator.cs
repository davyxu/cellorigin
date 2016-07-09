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

            _gamePeer.RegisterMessage<gamedef.ModelDataACK>(obj =>
            {
                var msg = obj as gamedef.ModelDataACK;

                var mdm = ModelDataManager.Instance;

                for( int i= 0;i<msg.Data.Count;i++)
                {
                    var data = msg.Data[i];

                    string key = null;


                    if (key == null)
                        continue;

                    
                }

            });

        }
    }


}