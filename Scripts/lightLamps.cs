using UnityEngine;
using System.Collections;

public class lightLamps : MonoBehaviour {
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Orb")
		{
			transform.GetComponent<Light>().enabled = true;
		}
	}
}
