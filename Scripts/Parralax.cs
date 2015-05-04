using UnityEngine;
using System.Collections;

public class Parralax : MonoBehaviour {

	private float xbackground;

	public float offset;
	public bool followCamera;
	public Camera cam;

	// Use this for initialization
	void Start () {
		xbackground = cam.transform.position.x;
	
	}
	
	// Update is called once per frame
	void Update () {
		if(followCamera)
		{
			float x = (cam.transform.position.x - xbackground)/offset;
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
			Debug.Log("Main cam position is " + cam.transform.position);
			Debug.Log("background position is " + transform.position);


		}
		else{
			float x = (xbackground - cam.transform.position.x)/offset;
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
		}
	
	}
}
