using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

public class LobbyController : NetworkManager {

	public bool _isAtStartup = false;

	public bool _localTest = true;

	List<MatchDesc> matchList = new List<MatchDesc>();

	public NetworkMatch networkMatch;
	public bool matchCreated;



	void Awake()
	{
		singleton = this;
		networkMatch = gameObject.AddComponent<NetworkMatch>();
	}
	
	void Start () {

	}

	void Update () {
	
	}

	public void StartServer() {
		if (_isAtStartup) {return;}

		if (gameObject.GetComponent<ServerEngine>() == null){
			gameObject.AddComponent<ServerEngine>();
		}

		_isAtStartup = true;

		//if (NetworkServer.Listen(7777)) {
		//	Debug.Log("Start listening " + NetworkManager.singleton.networkPort + " port...");
		//}
	}


	public virtual void OnServerConnect(NetworkConnection conn) {
		Debug.Log("Connect: " + conn.address);
	}

	public void StartClient() {
		if (_isAtStartup) {return;}

		gameObject.AddComponent<ClientEngine>();

//		Debug.Log("Trying to connect...");
//
//		_client = new NetworkClient();
//		_client.RegisterHandler(MsgType.Connect, OnConnected);    
//		_client.RegisterHandler(MsgType.Error, OnError);    
//		_client.Connect("127.0.0.1", 7777);



		//networkMatch.JoinMatch(match.networkId, "", OnMatchJoined);

		_isAtStartup = true;
	}

	void OnApplicationQuit(){
		NetworkServer.Shutdown();
	}
}
