using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GarageShapeChanging : MonoBehaviour {

	public bool locked = false;

	public void ButtonClickingFunction()
	{

		if (!locked) {
			
			FindObjectOfType<playerObj>().setShape(GetComponentInChildren<Text> ().text);
			Destroy (GameObject.FindGameObjectWithTag ("Player").transform.GetChild (0).gameObject);
			FindObjectOfType<loadAgentShape> ().LoadAgent ();
			FindObjectOfType<GarageManager> ().changeShape ();
		} else 
		{
			FindObjectOfType<loadAgentShape> ().displayAgent (GetComponentInChildren<Text> ().text);
			FindObjectOfType<itemBank> ().getLockCondition (GetComponentInChildren<Text> ().text);
		}
	}


	public void setLocked()
	{
		locked = true;
	}
}
