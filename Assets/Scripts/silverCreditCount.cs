using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class silverCreditCount : MonoBehaviour {

	void Awake()
	{
		refresh ();
	}

	public void refresh()
	{
		GetComponent<Text> ().text = FindObjectOfType<playerObj>().silverCredits.ToString();
	}
}
