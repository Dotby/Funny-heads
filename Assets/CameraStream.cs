using UnityEngine;
using System.Collections;

public class CameraStream : MonoBehaviour {

	//http://www.turbosquid.com/3d-model/cartoon-head/
	//heads models

	public WebCamTexture mCamera = null;
	public GameObject plane;

	IEnumerator Start() {
		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);

		if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
			InitCamTexture();
			Debug.Log("Camera status: OK.");
		}
		else 
		{
			Debug.Log("Camera status: error.");
		}
	}
	
	void InitCamTexture ()
	{
		if (plane == null){
			Debug.Log("No plane!");
			return;
		}
		
		mCamera = new WebCamTexture ();

		if (Application.platform == RuntimePlatform.IPhonePlayer){

			Vector3 _old = plane.transform.localScale;

			plane.transform.localScale = new Vector3(-1.0f * _old.x, _old.y, 1.0f);
		}

		plane.GetComponent<Renderer>().material.mainTexture = mCamera;
		mCamera.Play ();
		
	}
}
