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
                    if ( data.HasKeyStr )
                    {
                        key = data.KeyStr;
                    }

                    if (key == null)
                        continue;


                    if ( data.HasInteger )
                    {                        
                        mdm.SetInteger(key, data.Integer, data.SyncBehavior);
                    }
                    else if ( data.HasStr )
                    {
                        mdm.SetString(key, data.Str, data.SyncBehavior);
                    }
                    else if (data.HasNumber)
                    {
                        mdm.SetNumber(key, data.Number, data.SyncBehavior);
                    }
                    else if (data.HasBool)
                    {
                        mdm.SetBool(key, data.Bool, data.SyncBehavior);
                    }
                    
                }

            });

        }
    }


}