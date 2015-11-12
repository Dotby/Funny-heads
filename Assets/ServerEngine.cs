using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

public class ServerEngine : NetworkBehaviour {
	
	public LobbyController _lobby;

	private GameObject _playerPrefab = null;

	void Start () {

		_playerPrefab = NetworkManager.singleton.playerPrefab;
		_lobby = gameObject.GetComponent<LobbyController>();
		Init ();
	}

	void Update () {
	
	}

//	private void SpawnPlayer(NetworkConnection conn) // spawn a new player for the desired connection
//	{
//		GameObject playerObj = GameObject.Instantiate(_playerPrefab); // instantiate on server side
//		NetworkServer.AddPlayerForConnection(conn, playerObj, 0); // spawn on the clients and set owner
//	}

	public void Init() {

		Debug.Log("Create host..");



		//URI Issue solver

		//_lobby.matchMaker.baseUri = null;

		Debug.Log("Base MM Uri: " + _lobby.matchMaker.baseUri);
		
		if (_lobby.matchMaker.baseUri == null){

			System.Uri _uri = new System.Uri(System.Uri.EscapeUriString("https://mm.unet.unity3d.com/"));
			_lobby.matchMaker.baseUri = _uri;
		}else{
			if (_lobby.matchMaker.baseUri.ToString() == "")
			{
				System.Uri _uri = new System.Uri(System.Uri.EscapeUriString("https://mm.unet.unity3d.com/"));
				_lobby.matchMaker.baseUri = _uri;
			}
		}

		Debug.Log("NEW Base MM Uri: " + _lobby.matchMaker.baseUri);

		//END

		if (!_lobby._localTest)
		{
			CreateMatchRequest create = new CreateMatchRequest();
			create.name = "Mixarine";
			create.size = 4;
			create.advertise = true;
			create.password = "";
			
			_lobby.matchMaker.CreateMatch(create, OnMatchCreate);
		}
		else{
			NetworkManager.singleton.StartServer();
		}
		
		StartCoroutine(WaitServerForStart());
	}

	IEnumerator WaitServerForStart() {
		
		yield return NetworkServer.active;
		
		NetworkServer.RegisterHandler(MsgType.Connect, ConnectClient);
		NetworkServer.RegisterHandler(MsgType.Disconnect, PlayerDisconnect);
		//NetworkServer.RegisterHandler(MsgType.AddPlayer, OnServerAddPlayer);

		//GameObject player = (GameObject)Instantiate(NetworkManager.singleton.playerPrefab, new Vector3(0f, 0f, 3.0f), Quaternion.identity);
		
		//short _id = player.GetComponent<NetworkIdentity>().playerControllerId;
		
		//Debug.Log("Id: " + _id);
		//NetworkServer.AddPlayerForConnection(_mes.conn, player, 0);


		//NetworkManager.singleton.OnServerAddPlayer(,);
		//NetworkServer.RegisterHandler(MsgType.Ready, OnPlayerReadyMessage);
		//NetworkServer.RegisterHandler(MsgType.AddPlayer, OnServerAddPlayer);
		
		Debug.Log("Server started.");
	}


	void PlayerDisconnect(NetworkMessage _msg){
		Debug.Log("Player disconnect.");
		NetworkServer.DestroyPlayersForConnection(_msg.conn);
	}

	public void OnMatchCreate(CreateMatchResponse matchResponse)
	{
		if (matchResponse.success)
		{
			Debug.Log("Create match succeeded.");
			_lobby.matchCreated = true;
			MatchInfo matchInfo  = new MatchInfo(matchResponse);
			Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));

			NetworkManager.singleton.StartHost(matchInfo) ;
			NetworkServer.Listen(new MatchInfo(matchResponse), 9000);
		}
		else
		{
			Debug.LogError ("Create match failed.");
		}
	}

	public void OnServerAddPlayer(NetworkMessage _mes)
	{
		Debug.Log("Add player.");
		GameObject player = (GameObject)Instantiate(NetworkManager.singleton.playerPrefab, new Vector3(0f, 0f, 3.0f), Quaternion.identity);

		short _id = player.GetComponent<NetworkIdentity>().playerControllerId;

		//Debug.Log("Id: " + _id);
		//NetworkServer.AddPlayerForConnection(_mes.conn, player, 0);

		NetworkServer.SpawnWithClientAuthority(player, _mes.conn);
	}

//	GameObject CreateObj(Vector3 _vec, UnityEngine.Networking.NetworkHash128 _assetID){
//		Debug.Log("Create.");
//		return NetworkManager.singleton.playerPrefab;
//	}
//
//	void DeleteObj(GameObject _obj){
//		Debug.Log("Delete.");
//	}
	

	//	public virtual void OnServerRemovePlayer(NetworkConnection conn, short playerControllerId)
//	{
//		PlayerController player;
//		if (conn.GetPlayer(playerControllerId, out player))
//		{
//			if (player.NetworkIdentity != null && player.NetworkIdentity.gameObject != null)
//				NetworkServer.Destroy(player.NetworkIdentity.gameObject);
//		}
//	}


	void ConnectClient(NetworkMessage _msgType) {
		Debug.Log("Client connected.");

		NetworkServer.SetClientReady(_msgType.conn);
		NetworkServer.SpawnObjects();

		//SpawnPlayer(_msgType.conn);
	

		///GameObject player = (GameObject)Instantiate(NetworkManager.singleton.playerPrefab, Vector3.zero, Quaternion.identity);
		//player.GetComponent<Player>().color = Color.Red;
		///NetworkServer.AddPlayerForConnection(_msgType.conn, player, 0);

		///NetworkServer.Spawn(player);
		//ClientScene.AddPlayer(0);

		StringMessage _txt = new StringMessage("Hello!");
		NetworkServer.SendToClient(_msgType.conn.connectionId, 100, _txt);
	}
}
