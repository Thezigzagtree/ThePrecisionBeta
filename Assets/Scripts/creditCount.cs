using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class creditCount : MonoBehaviour {

	void Awake()
	{
		refresh ();
	}

	public void refresh()
	{
		GetComponent<Text> ().text = SaveSystem.GetInt("credits").ToString();}//FindObjectOfType<playerObj>().credits.ToString();	}
}
