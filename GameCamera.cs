using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	
	private Transform target, cameraPosition;
	private float trackSpeed = 10;
	
	
	// Set target
	public void SetTarget(Transform t) {
		target = t;
	}

	// Set camera
	public void SetCamera(Transform t) {
		cameraPosition = t;
	}
	
	// Track target
	void LateUpdate() {
		if (target) {
			float x = IncrementTowards(cameraPosition.position.x, target.position.x, trackSpeed);
			float y = IncrementTowards(cameraPosition.position.y, target.position.y + 2, trackSpeed);
			cameraPosition.position = new Vector3(x,y, cameraPosition.position.z);
		}
	}
	
	// Increase n towards target by speed
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		} else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}
}
