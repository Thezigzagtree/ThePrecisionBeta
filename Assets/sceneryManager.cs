using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class sceneryManager : MonoBehaviour {

	public Material lineMat;
	public Material disableMat;
	public Material stageLockMat;
	public Material wrongMat;


	public bool checkSceneryUnlocked()
	{
		for (int i = 0; i < gameObject.transform.childCount; i++) 
		{
			if (gameObject.transform.GetChild (i).GetComponent<MeshRenderer> ().sharedMaterial == disableMat) {
				Debug.Log (gameObject.transform.GetChild (i).name + " is still locked");
				return false;
			}

		}

		return true;
				
	}

}


