using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setRandomCost : MonoBehaviour {

	void Start()
	{
		GetComponentInChildren<Text> ().text = FindObjectOfType<storeManager> ().getRandomItemCost ().ToString ();
	}
}
