using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerControllerAnimate : MonoBehaviour {

	//Note to self: Need to add a function to physics to change the collider size, eg for player controller & orb

	//Include proteceted Animatro object
	protected Animator animator;
	//detect facing
	public bool lookRight = false;
	
	// Player Handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
	public float jumpHeight = 12;

	//Speed left/right & up/down
	private float currentHorizontalSpeed;
	private float targetHorizontalSpeed;
	private float currentVerticalSpeed;
	private float targetVerticalSpeed;
	private Vector2 amountToMove;
	
	private PlayerPhysics playerPhysics;
	private GameCamera cam; //Link to camera script
	public GameObject mainCamera; // Refernce to scenes main camera

	public List<GameObject> orbs = new List<GameObject>(); //Collection of orbs
	public GameObject orbPrefab;
	private int selectedIndex = 0;
	private Transform currentObject; //This holds the transform for the current controlled objects, either player or orb
	
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();

		cam = GetComponent<GameCamera>();

		cam.SetCamera (mainCamera.gameObject.transform);
		cam.SetTarget (transform);

		currentObject = transform;

		//Create initial orb
		addOrb ();
		addOrb ();
		addOrb ();

		//Get animator component
		animator = GetComponent<Animator>();

	}
	
	void Update () {
		// Reset acceleration upon collision
		if (playerPhysics.movementStopped) {
			targetHorizontalSpeed = 0;
			currentHorizontalSpeed = 0;
			targetVerticalSpeed = 0;
			currentVerticalSpeed = 0;
		}
		
		// If player is touching the ground
		if (playerPhysics.grounded) {
			amountToMove.y = 0;
			//jump is false
			animator.SetBool("Jump" , false);
			
			// Jump
			if (Input.GetButtonDown("Jump")) {
				//animate jump
				animator.SetBool("Jump" , true);
				amountToMove.y = jumpHeight;	
			}
		}
		
		// Input for player and orb
		targetHorizontalSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentHorizontalSpeed = IncrementTowards(currentHorizontalSpeed, targetHorizontalSpeed, acceleration);
		
		// Set amount to move
		amountToMove.x = currentHorizontalSpeed;
		amountToMove.y -= gravity * Time.deltaTime;

		//assign speed
		animator.SetFloat("Speed",currentHorizontalSpeed);



		//flip direction
		if(currentHorizontalSpeed > 0 && lookRight == false){ // some condition to rotate 180
			lookRight = true;
			animator.SetBool("Look Right", lookRight);
			foreach (Transform child in transform)
			{
				child.transform.RotateAround (transform.position, transform.up, 180f);
			}
		}
		else if (currentHorizontalSpeed < 0 && lookRight == true){
			foreach (Transform child in transform)
			{
				child.transform.RotateAround (transform.position, transform.up, 180f);
			}
			lookRight = false;
			animator.SetBool("Look Right", lookRight);
		}

		
		if (playerPhysics.isOrb) {
			// Vertical input for orb
			targetVerticalSpeed = Input.GetAxisRaw("Vertical") * speed;
			currentVerticalSpeed = IncrementTowards(currentVerticalSpeed, targetVerticalSpeed, acceleration);
			amountToMove.y = currentVerticalSpeed;
		}


		playerPhysics.Move(currentObject, amountToMove * Time.deltaTime);

		//Check for player key input

		//When tab key is pressed itterate through the orbs
		if (Input.GetKeyUp (KeyCode.Tab)) {

			if(selectedIndex < orbs.Count){
				currentObject = orbs[selectedIndex].gameObject.transform;
				playerPhysics.isOrb = true;
				cam.SetTarget (currentObject);
				selectedIndex += 1;
			} else {
				selectedIndex = 0;
				currentObject = transform;
				playerPhysics.isOrb = false;
				cam.SetTarget (currentObject);
			}


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

	//Function is used to instatiate a new orb
	private void addOrb(){
		//TODO: Make this position dynamic
		Vector3 orbPos = transform.position;
		orbPos.y += 1;
		orbPos.x -= (1 + orbs.Count);

		GameObject Orb = (GameObject)Instantiate (orbPrefab, orbPos, transform.rotation);
		orbs.Add (Orb);
	}
}
