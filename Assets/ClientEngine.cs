using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

public class ClientEngine : NetworkBehaviour {

	LobbyController _lobby;
	NetworkClient _client;

	void Start () {

		_lobby = gameObject.GetComponent<LobbyController>();
		Init();
	}

	void Update () {
	
	}

	void Init(){

		if (!_lobby._localTest){
			_lobby.matchMaker.ListMatches(0, 20, "", OnMatchList);
		}else{
			NetworkManager.singleton.StartClient();
		}
	}

//	void OnError(NetworkMessage _netMsg) {
//		int _err = _netMsg.ReadMessage<IntegerMessage>().value;
//		Debug.Log("Error: " + _err);	
//		//_isAtStartup = false;
//	}
	
	//client
	void OnConnected(NetworkMessage _netMsg) {

		//NetworkClient.Instance.Ready();

		ClientScene.Ready(_netMsg.conn);

		Debug.Log("Connected.");

		if (ClientScene.ready){
			Debug.Log("Client ready to spawn.");
			ClientScene.AddPlayer(_netMsg.conn, 0);
		}else{
			Debug.Log("Client NOT ready to spawn.");
		}
		
		//_isAtStartup = false;
	}

	public void OnMatchList(ListMatchResponse matchListResponse)
	{
		if (matchListResponse.success && matchListResponse.matches != null)
		{
			_lobby.matchMaker.JoinMatch(matchListResponse.matches[0].networkId, "", OnMatchJoined);
		}
	}

	public void OnMatchJoined(JoinMatchResponse matchJoin)
	{
		if (matchJoin.success)
		{
			Debug.Log("Join match succeeded");
			if (_lobby.matchCreated)
			{
				Debug.LogWarning("Match already set up, aborting...");
				return;
			}

			MatchInfo matchInfo = new MatchInfo(matchJoin);
			Utility.SetAccessTokenForNetwork(matchJoin.networkId, new NetworkAccessToken(matchJoin.accessTokenString));
			_client = new NetworkClient();
			_client.RegisterHandler(MsgType.Connect, OnConnected);
			_client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
			_client.RegisterHandler(MsgType.Error, OnError);
			_client.RegisterHandler(MsgType.AddPlayer, AddPlayerMessage);
			//_client.RegisterHandler(MsgType.Owner, OwnerMes);
			_client.RegisterHandler(100, MesFromServer);

			NetworkManager.singleton.StartClient(matchInfo);
			//_client.Connect(matchInfo);
		}
		else
		{
			Debug.LogError("Join match failed.");

		}
	}

	void AddPlayerMessage(NetworkMessage _mes){
		Debug.Log("Add player.");
	}

	void OwnerMes(NetworkMessage _mes){
		Debug.Log("Owner mes.");
	}

//	void OnStartLocalPlayer(){
//		Debug.Log("On start local player.");
//		//ClientScene.AddPlayer();
//	}

	//ClientScene.RegisterSpawnHandler(prefab.GetCompoment<NetworkIdentity>().assetId, createFunc, destroyFunc)

	void OnDisconnect(NetworkMessage _msg){
		Debug.Log("Disconnect.");
		_lobby._isAtStartup = false;
		this.enabled = false;
	}

	void OnError(NetworkMessage _netMsg) {
		if (_netMsg.ReadMessage<IntegerMessage>().value == 6){
			Debug.Log("TimeOut.");
			NetworkClient.ShutdownAll();

			_lobby._isAtStartup = false;
			this.enabled = false;
		}

	}
	
	void MesFromServer(NetworkMessage _netMsg) {
		string _txt = _netMsg.ReadMessage<StringMessage>().value;
		
		Debug.Log("Server said: " + _txt);

	}

}
