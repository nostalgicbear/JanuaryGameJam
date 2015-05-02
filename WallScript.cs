using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {

	public GameObject wallObject;
	
	private bool switchState = false;
	private bool changed = false;
	public float timer = 0.2f;
	AudioSource powerOn;
	AudioSource powerDown;
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.tag == "Orb") {

			if(!switchState){
				powerOn.Play ();
			}

			switchState = true;
			timer = 0.2f;
			changed = true;
		}	
	}

	// Use this for initialization
	void Start () {
		AudioSource[] audios = GetComponents<AudioSource> ();

		powerOn = audios [0];
		powerDown = audios [1];
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0){
			if(switchState){
				powerOn.Stop ();
				powerDown.Play();
			}
			switchState = false;
			timer = 0.2f;
		}
		
		if (switchState) {
			
			wallObject.gameObject.collider.enabled = false;
			wallObject.gameObject.renderer.enabled = false;
		}
		
		if (changed && !switchState) {
			wallObject.gameObject.collider.enabled = true;
			wallObject.gameObject.renderer.enabled = true;
		}
		
		
	}
}
