using UnityEngine;
using System.Collections;

public class CardboardController : MonoBehaviour {

	public GameObject _cam;

	// Use this for initialization
	void Start () {

#if UNITY_STANDALONE
		Debug.Log("Cardboard OFF");
		gameObject.SetActive(false);
		_cam.SetActive(true);
		return;
#endif

		Cardboard.SDK.OnTrigger += RecenterCardboard;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void RecenterCardboard(){
		Cardboard.SDK.Recenter();
	}
}
