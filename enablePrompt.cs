using UnityEngine;
using System.Collections;

public class enablePrompt : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			GameObject.FindGameObjectWithTag("lampPrompt_text").GetComponent<GUIText>().enabled = true;
		}
	}
}
