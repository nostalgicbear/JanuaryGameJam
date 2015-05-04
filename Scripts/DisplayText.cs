using UnityEngine;
using System.Collections;

public class DisplayText : MonoBehaviour {
	private string str;
	public string yourText;
	public int lengthTextStaysOnScreen;

	void Start(){
		StartCoroutine(AnimateText(yourText));
	}

	IEnumerator AnimateText(string strComplete){
		int i = 0;
		str = "";

		while(i < strComplete.Length){
			str+= strComplete[i++];
			yield return new WaitForSeconds(0.1f);
			guiText.text = str;
		}
		yield return new WaitForSeconds(lengthTextStaysOnScreen);
		guiText.enabled = false;
	}

	/**
	void OnGUI(){ 
		GUI.Box(new Rect(10,400,1260,225), str); 
	}
	*/

}
