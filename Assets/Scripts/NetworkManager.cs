using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour {
	NetworkClient myClient;
	// Use this for initialization
	void Start () {
		SetupServer();
		SetupClient();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetupServer(){
		NetworkServer.Listen(80);
	}

	public void SetupClient(){
		myClient = new NetworkClient();
		myClient.RegisterHandler(MsgType.Connect, OnConnected);
		myClient.Connect("192.168.1.83", 80);
	}
	
	public void OnConnected(NetworkMessage netMsg)
    {
    	Debug.Log("SUCCESS");
        Debug.Log("Connected to server");
    }
}
