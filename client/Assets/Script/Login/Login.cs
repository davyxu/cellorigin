using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : MonoBehaviour {

    public NetworkPeer peer;

	// Use this for initialization
	void Start () {
        peer.RegisterMessage<proto.PeerConnected>(msg =>
       {
           Debug.Log("connected");

           var req = new proto.LoginREQ();
           req.PlatformToken = "hello";

           peer.SendMessage(req);
       });

        peer.RegisterMessage<proto.LoginACK>(raw =>
        {
            var ack = raw as proto.LoginACK;

            Debug.Log(string.Format("login ok {0}", ack.Token));
        });
    }
	
	// Update is called once per frame
	void Update () {
	    
        
	}

    private void OnDestroy()
    {
        peer.Stop();
    }

    void OnGUI()
    {
        if ( GUI.Button(new Rect(10, 10, 50, 50), "hello"))
        {
            peer.Connect("127.0.0.1:8001");
            UnityEngine.Debug.Log("login");
        }
    }
}
