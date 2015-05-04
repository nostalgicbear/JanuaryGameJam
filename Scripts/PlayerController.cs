using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerController : MonoBehaviour {

	//Note to self: Need to add a function to physics to change the collider size, eg for player controller & orb

	//Include proteceted Animatro object
	protected Animator animator;
	//detect facing
	public bool lookRight = false;
	public bool orbControlled = false;

	// Player Handling
	public float gravity = 20;
	public float speed = 8;
	public float acceleration = 30;
	public float jumpHeight = 12;

	//Speed left/right & up/down
	private float currentHorizontalSpeed;
	public float targetHorizontalSpeed;
	private float currentVerticalSpeed;
	public float targetVerticalSpeed;
	private Vector2 amountToMove;
	
	private PlayerPhysics playerPhysics;
	public GameCamera cam; //Link to camera script
	public GameObject mainCamera; // Refernce to scenes main camera

	public List<GameObject> orbs = new List<GameObject>(); //Collection of orbs
	public GameObject orbPrefab;
	private int selectedIndex = 0;
	private int followCount;
	private OrbScript accessOrb; //Access point to the current orbs script
	private Transform currentObject; //This holds the transform for the current controlled objects, either player or orb
	public bool onPlatform = false;
	public bool freezePosition = false;
	
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics>();

		cam = GetComponent<GameCamera>();

		cam.SetCamera (mainCamera.gameObject.transform);
		cam.SetTarget (transform);

		currentObject = transform;

		//Create initial orb
		//TODO: Make this position dynamic
		Vector3 orbPos = transform.position;
		orbPos.y += 1;
		orbPos.x -= (1 + orbs.Count);

		/*
		//Add 3 orbs for debuging
		addOrb(orbPos);
		orbPos.x -= (1 + orbs.Count);

		addOrb(orbPos);
		orbPos.x -= (1 + orbs.Count);

		addOrb(orbPos);

	*/

	


		//Get animator component
		animator = GetComponent<Animator>();


	}
	
	void Update () {
	if(!freezePosition){

		// Reset acceleration upon collision
		if (playerPhysics.movementStopped) {
			targetHorizontalSpeed = 0;
			currentHorizontalSpeed = 0;
			targetVerticalSpeed = 0;
			currentVerticalSpeed = 0;
		}
		
		// If player is touching the ground
		if (playerPhysics.grounded || onPlatform) {
			amountToMove.y = 0;

			//jump is false
			animator.SetBool("Jump" , false);

			// Jump
			if (Input.GetButtonDown("Jump")) {
				amountToMove.y = jumpHeight;	
				animator.SetBool("Jump" , true);
			}
		}
		
		// Input for player and orb
		targetHorizontalSpeed = Input.GetAxisRaw("Horizontal") * speed;
		currentHorizontalSpeed = IncrementTowards(currentHorizontalSpeed, targetHorizontalSpeed, acceleration);
		
		// Set amount to move
		amountToMove.x = currentHorizontalSpeed;

		if (!onPlatform) {
			amountToMove.y -= gravity * Time.deltaTime;
		}

		
		if (playerPhysics.isOrb) {

			accessOrb = (OrbScript) orbs[selectedIndex-1].GetComponent(typeof(OrbScript));

			if(targetHorizontalSpeed != 0 || targetVerticalSpeed != 0 && accessOrb.isFollowing){
				accessOrb.isFollowing = false;
				accessOrb.enableTrail(true);
				accessOrb.changeCollider(true);
				accessOrb.setLightRange(8);
				organiseOrbs();
			}



			// Vertical input for orb
			targetVerticalSpeed = Input.GetAxisRaw("Vertical") * speed;
			currentVerticalSpeed = IncrementTowards(currentVerticalSpeed, targetVerticalSpeed, acceleration);
			amountToMove.y = currentVerticalSpeed;
		}


		playerPhysics.Move(currentObject, amountToMove * Time.deltaTime);
		
		//Check for player key input

		if(Input.GetKeyDown (KeyCode.Tab)){
			gameObject.GetComponent<Animator>().enabled = false;
		}

		//When tab key is pressed itterate through the orbs
		if (Input.GetKeyUp (KeyCode.Tab)) {


			//If the next object is an orb otherwise focus on the player
			if(selectedIndex < orbs.Count){
				currentObject = orbs[selectedIndex].gameObject.transform;
				playerPhysics.isOrb = true;
				animator.SetBool("orbControlled", playerPhysics.isOrb);
				cam.SetTarget (currentObject);

				//Gets access to current orbs script
				accessOrb = (OrbScript) orbs[selectedIndex].GetComponent(typeof(OrbScript));
				playerPhysics.SetCollider(new Vector3(0.3f, 0.6f, 0.3f), new Vector3(0.0f, 0.0f, 0.0f));

				selectedIndex += 1;
			} else {
				gameObject.GetComponent<Animator>().enabled = true;
				setFocusToPlayer();
				setupPlayerCollider();
				animator.SetBool("orbControlled", playerPhysics.isOrb);
			}


		}

		if (Input.GetKeyUp (KeyCode.F) && playerPhysics.isOrb){
			accessOrb = (OrbScript) orbs[selectedIndex-1].GetComponent(typeof(OrbScript));
			accessOrb.isFollowing = true;
			accessOrb.changeCollider(false);
			accessOrb.enableTrail(false);
			setFocusToPlayer();
			gameObject.GetComponent<Animator>().enabled = true;
			setupPlayerCollider();
			
			accessOrb.playerTransform = currentObject;
			organiseOrbs();
			animator.SetBool("orbControlled", playerPhysics.isOrb);




		}


		//Animation handling

		//Assign speed
		animator.SetFloat("Speed",currentHorizontalSpeed);
		
	
		if(!playerPhysics.isOrb){
		//flip direction
			if(currentHorizontalSpeed > 0 && lookRight == false){ // some condition to rotate 180
				lookRight = true;
				organiseOrbs();
				animator.SetBool("Look Right", lookRight);
				foreach (Transform child in transform){
					child.transform.RotateAround (transform.position, transform.up, 180f);
				}

				playerPhysics.SetColliderCentre(new Vector3(0.02f, 0.5f, 0.0f));
				
			} else if (currentHorizontalSpeed < 0 && lookRight == true){

				foreach (Transform child in transform) {
					child.transform.RotateAround (transform.position, transform.up, 180f);
				}

				playerPhysics.SetColliderCentre(new Vector3(-0.05f, 0.5f, 0.0f));

				lookRight = false;
				organiseOrbs();
				animator.SetBool("Look Right", lookRight);
			}
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

	//Added to reduce duplicate code, this function resets the current index and focuses back to player
	private void setFocusToPlayer(){
			selectedIndex = 0;
			currentObject = transform;
			playerPhysics.isOrb = false;
			cam.SetTarget (currentObject);
	}

	//This function is used to re-order the orbs following the player
	private void organiseOrbs(){

		followCount = 0;

		//Update follow count
		for(int o = 0; o < orbs.Count; o++){
			accessOrb = (OrbScript) orbs[o].GetComponent(typeof(OrbScript));
			
			if(accessOrb.isFollowing){
				followCount += 1;
				accessOrb.index = followCount;
				accessOrb.setLightRange(3);
				accessOrb.rightSide = lookRight;
			}
		}
	}

	private void setupPlayerCollider(){
		//Set collider back up once player controlled again
		if(lookRight){
			playerPhysics.SetCollider(new Vector3(0.3f, 0.9f, 0.3f), new Vector3(0.02f, 0.5f, 0.0f));
		} else {
			playerPhysics.SetCollider(new Vector3(0.3f, 0.9f, 0.3f), new Vector3(-0.05f, 0.5f, 0.0f));
		}
	}
	
	//Function is used to instatiate a new orb
	public void addOrb(Vector3 spawnPosition){

		GameObject Orb = (GameObject)Instantiate (orbPrefab, spawnPosition, transform.rotation);
		accessOrb = (OrbScript) Orb.GetComponent(typeof(OrbScript));
		accessOrb.playerTransform = transform;
		accessOrb.index = followCount + 1;
		orbs.Add (Orb);

		followCount += 1;

		organiseOrbs ();
	}
}
