using UnityEngine;
using System.Collections;

public class DeadOrbScript : MonoBehaviour {

	public float targetHeight;
	public float Speed;
	private bool animate = false;
	private bool createNewOrb = false;
	private bool orbCreated = false;
	private GameObject player;

	AudioSource idleAudio;
	AudioSource aliveAudio;

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Player") {

			idleAudio.Stop();
			aliveAudio.Play ();
			//Freeze player
			animate = true;
			player = other.gameObject;
			player.GetComponent<PlayerController>().freezePosition = true;
			player.GetComponent<Animator>().enabled = false;
			//Move self into air
		}	
	}


	void FixedUpdate() {

		// If Switch becomes true, it tells the platform to move to its Origin.
		if(animate && transform.position.y < targetHeight){
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x,targetHeight,transform.position.z), Speed);
		} else if (transform.position.y >= targetHeight){
			transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
			createNewOrb = true;
		}

		if(createNewOrb && !orbCreated){
			orbCreated = true;
			createNewOrb = false;
			Debug.Log("Adding Orb");
			player.GetComponent<PlayerController>().addOrb(transform.position);
			player.GetComponent<PlayerController>().freezePosition = false;
			player.GetComponent<Animator>().enabled = true;

			Destroy(gameObject);
		}
	}
	

	// Use this for initialization
	void Start () {
		AudioSource[] audios = GetComponents<AudioSource> ();
		idleAudio = audios [0];
		aliveAudio = audios [1];

		idleAudio.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
