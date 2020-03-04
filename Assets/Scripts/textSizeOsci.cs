using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textSizeOsci : MonoBehaviour {

	public int wobble;
	public int baseFont;

	private List <System.Action> ToAnimate = new List<System.Action>();

	public void startWobble()
	{
		GetComponent<Text> ().fontSize = (Mathf.FloorToInt (Mathf.Sin(Time.timeSinceLevelLoad * 5) * wobble) + baseFont);
	}

	public void addWobble()
	{
		ToAnimate.Add (startWobble);
	}

	void Update()
	{
		for (int i = 0; i < ToAnimate.Count; i++) {
			ToAnimate [i] ();	
		}

	}
}
