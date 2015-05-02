using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {

	private float lastX;
	public float playerOffset;
	public bool isActive;
	public bool verticalOnly;

	void OnTriggerStay(Collider other) {
		if(other.gameObject.tag == "Player") {
			//platform.gameObject.transform.position = Vector3.MoveTowards(platform.gameObject.transform.position, OriginSpot.position, Speed);
			other.GetComponent<PlayerController>().onPlatform = true;

			if(isActive){

				Vector2 amountToMove;
				amountToMove.y = 0;

				if(lastX > transform.position.x){
					amountToMove.x = -playerOffset;

				} else {
					amountToMove.x = playerOffset;
				}

				if(!verticalOnly){
					Debug.Log ("Not vertical only");
					other.GetComponent<PlayerPhysics>().Move(other.transform, amountToMove * Time.deltaTime);
				}

				other.transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
				lastX = transform.position.x;
			}
		}	
	}

	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag == "Player") {
			other.GetComponent<PlayerController>().onPlatform = false;
		}	
	}

	// Use this for initialization
	void Start () {
		lastX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {

	}

}
