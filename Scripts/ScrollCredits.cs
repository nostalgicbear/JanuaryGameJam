using UnityEngine;
using System.Collections;

public class ScrollCredits : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.y > 0.3534)
		{
			GameObject.FindGameObjectWithTag("EndGameText").GetComponent<GUIText>().enabled = false;
		}

		if(transform.position.y < 0.5217762)
		{
			transform.Translate(Vector3.up * Time.deltaTime * 0.1f);
		}
	
	}
}
