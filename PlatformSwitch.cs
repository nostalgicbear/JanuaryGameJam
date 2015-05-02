using UnityEngine;
using System.Collections;

public class PlatformSwitch : MonoBehaviour {

	public Transform DestinationSpot;
	public Transform OriginSpot;
	public GameObject platform;
	public bool verticalOnly;
	public float Speed;
	public bool Switch = false;
	private bool first = false;

	private bool switchState = false;
	private bool changed = false;
	public float timer = 0.2f;
	AudioSource powerOn;
	AudioSource powerDown;

	
	
	// Use this for initialization
	void Start () {
		AudioSource[] audios = GetComponents<AudioSource> ();
		powerOn = audios [0];
		powerDown = audios [1];

	}

	void FixedUpdate() {


		timer -= Time.deltaTime;
		if(timer <= 0){

			if(switchState){
				powerOn.Stop ();
				powerDown.Play();
			}
			switchState = false;
			timer = 0.2f;
		}

		// For these 2 if statements, it's checking the position of the platform.
		// If it's at the destination spot, it sets Switch to true.
		if(switchState && platform.gameObject.transform.position == DestinationSpot.position){
			Switch = true;
		}
		if(switchState && platform.gameObject.transform.position == OriginSpot.position){
			Switch = false;
		}
		
		// If Switch becomes true, it tells the platform to move to its Origin.
		if(switchState && Switch){
			platform.gameObject.transform.position = Vector3.MoveTowards(platform.gameObject.transform.position, OriginSpot.position, Speed);
			platform.GetComponentInChildren<PlatformScript>().isActive = true;
		}
		else if(switchState){
			platform.gameObject.transform.position = Vector3.MoveTowards(platform.gameObject.transform.position, DestinationSpot.position, Speed);
			platform.GetComponentInChildren<PlatformScript>().isActive = true;
		} else {
			platform.GetComponentInChildren<PlatformScript>().isActive = false;
		}
	}

	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.tag == "Orb") {
			Debug.Log ("Switch activated");

			if(!switchState){
				powerOn.Play ();
			}

			switchState = true;
			timer = 0.2f;
			changed = true;


		}	
	}
	

	// Update is called once per frame
	void Update () {

		
	}
}
