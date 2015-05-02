using UnityEngine;
using System.Collections;

public class TriggerForestHint : MonoBehaviour {

	public string textToUse;

	void OnTriggerEnter(Collider other)
	{
		
		if(other.gameObject.tag == "Player")
		{
			GameObject.FindGameObjectWithTag("ScreenText").guiText.text = textToUse;
			GameObject.FindGameObjectWithTag("ScreenText").guiText.enabled = true;
			//GameObject.FindGameObjectWithTag("ScreenText").GetComponent<DisplayText>().enabled = true;
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
