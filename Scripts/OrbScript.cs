using UnityEngine;
using System.Collections;

public class OrbScript : MonoBehaviour {

	public bool isFollowing = true;
	public int index;
	public Transform playerTransform; //Players transform so orb can follow
	public bool rightSide = false;

	private float distanceBuffer = 0.8f; //Distance between orbs and player
	private Vector3 targetPosition;
	private float trackSpeed = 10; //Speed at which orb follows player


	//TODO: Update so trail only appears when you are controlling an orb

	// Use this for initialization
	void Start () {
		enableTrail (false);
		setLightRange (3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setLightRange(float range) {

		foreach (Transform child in transform) {
			if(child.name != "orbLight"){
				continue;
			}
			
			child.GetComponent<Light>().range = range;
		}
	}


	public void enableTrail(bool value){
		foreach (Transform child in transform) {
			if(child.name != "Trail"){
				continue;
			}
			
			child.GetComponent<TrailRenderer>().enabled = value;
		}
	}

	// Track target
	void LateUpdate() {
		if (isFollowing) {

			float x;

			if(!rightSide){
				 x = IncrementTowards(transform.position.x, playerTransform.position.x + ((distanceBuffer*index)), trackSpeed);
			} else {
				 x = IncrementTowards(transform.position.x, playerTransform.position.x - ((distanceBuffer*index)), trackSpeed);
			}

			float y = IncrementTowards(transform.position.y, playerTransform.position.y + (2f), trackSpeed);

			transform.position = new Vector3(x,y, transform.position.z);
		}
	}

	// Increase n towards target by speed (Utility function)
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}

	//Used to turn off the orbs collider when following the player (avoid getting stuck)
	public void changeCollider(bool state) {
		gameObject.collider.enabled = state;
	}
}
