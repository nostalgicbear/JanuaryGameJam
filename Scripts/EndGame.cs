using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	private int counter = 0;
	public Material daytime;
	public GameObject Player;
	private bool gameOver;
	void OnTriggerEnter(Collider other)
	{



		if(other.gameObject.tag == "Orb")
		{
			counter+=1;
			Debug.Log("Counter increased to : " + counter);

		}

		if(counter >=3 && ( (transform.position.x - Player.transform.position.x) < 7))
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().freezePosition = true;
			GameObject endPos = new GameObject();
			endPos.transform.position = new Vector3(93.5f, 6.6f, -8.7f);
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().cam.SetTarget(endPos.transform);

			GameObject.FindGameObjectWithTag("EndGameText").GetComponent<GUIText>().enabled = true;
			gameOver = true;

		}


	}

	void Update() {

		if(counter>=3 && ( (transform.position.x - Player.transform.position.x) < 7))
		{
			GameObject.FindGameObjectWithTag("EndGameLight").GetComponent<Light>().enabled = true;
			GameObject.FindGameObjectWithTag("EndGameLight").GetComponent<Light>().intensity += 0.001f;
		}

		if(GameObject.FindGameObjectWithTag("EndGameLight").GetComponent<Light>().intensity > 1.0f)
		{
			RenderSettings.skybox = daytime;
			GameObject.FindGameObjectWithTag("EndGameLight").GetComponent<Light>().intensity = 1.0f;
		}

		if(gameOver)
		{
			GameObject.FindGameObjectWithTag("Credits").GetComponent<GUIText>().enabled = true;
			GameObject.FindGameObjectWithTag("Credits").GetComponent<ScrollCredits>().enabled = true;
		}
	}

	void Start(){
		Player = GameObject.Find("Player");
		gameOver = false;
	}
	

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Orb")
		{
			if(counter>0)
			{
				counter-=1;
				Debug.Log("Counter decreased to : " + counter);
			}
		}
	}

}
