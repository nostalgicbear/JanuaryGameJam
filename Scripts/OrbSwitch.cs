using UnityEngine;
using System.Collections;

public class OrbSwitch : MonoBehaviour {

	public GameObject effectedObject;

	private bool switchState = false;
	private bool changed = false;
	public float timer = 0.2f;

	void OnTriggerStay(Collider other) {
		if(other.gameObject.tag == "Orb") {
			Debug.Log ("Switch activated");
			switchState = true;
			timer = 0.2f;
			changed = true;
		}	
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0){
			switchState = false;
			timer = 0.2f;
		}

		if (switchState) {

			effectedObject.gameObject.collider.enabled = false;
			effectedObject.gameObject.renderer.enabled = false;
		}

		if (changed && !switchState) {
			effectedObject.gameObject.collider.enabled = true;
			effectedObject.gameObject.renderer.enabled = true;
		}


	}
}
