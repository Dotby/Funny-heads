using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class PlayerHeadController : NetworkBehaviour {

	Transform _headCam;
	
	void Start () {
		Debug.Log("Hi im head.");
			
		_headCam = GameObject.Find("Head").transform;

		Transform _spawnPoint = GameObject.Find("HeadPoint").transform;
			
		gameObject.transform.SetParent(_spawnPoint);
		gameObject.transform.localScale = Vector3.one;			
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	void LateUpdate () {
		if (isLocalPlayer){
			//Debug.Log("cam vec: " +  _headCam.localRotation);
			transform.localRotation = _headCam.localRotation;
		}
	}

	public override void OnStartLocalPlayer()
	{
		base.OnStartLocalPlayer();
		Debug.Log("Start local player.");

		GetComponent<Renderer>().enabled = false;

//		Transform _head = GameObject.Find("Head").transform;
//
//		gameObject.transform.SetParent(_head);

//		Transform _spawnPoint = GameObject.Find("HeadPoint").transform;
//		
//		gameObject.transform.SetParent(_spawnPoint);
//		gameObject.transform.localScale = Vector3.one;
//		gameObject.transform.localPosition = Vector3.zero;
//		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

		//gameObject.transform.localPosition = new Vector3(0f, -0.37f, 0.9f);
		//gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
		//gameObject.transform.rotation = Quaternion.Euler(0f, -180.0f, 0f);
	}
}
