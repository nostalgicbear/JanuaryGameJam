using UnityEngine;
using System.Collections;

public class TriggerLampHint : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			GameObject.FindGameObjectWithTag("ScreenText").guiText.enabled = true;
			GameObject.FindGameObjectWithTag("ScreenText").GetComponent<DisplayText>().enabled = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			GameObject.FindGameObjectWithTag("ScreenText").guiText.enabled = false;
			//GameObject.FindGameObjectWithTag("ScreenText").GetComponent<DisplayText>().enabled = true;
		}
	}
}
