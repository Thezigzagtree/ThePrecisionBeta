using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;
public class OptionsToggle : MonoBehaviour {

	public Agent agent;
	private bool vis = false;
	private bool mov = false;
	private Vector3 og_pos;
	private List <System.Action> ToAnimate = new List<System.Action>();

	void Awake () {

		og_pos = GetComponent<RectTransform>().localPosition;
		agent.OnMoveStart += hideOptions;
	}

	public void hideOptions (Node targetNode)
	{
		ToAnimate.Add (removeOptions);
	}

	public void showOptions()
	{
		if (vis == false && mov == false)
		{
			mov = true;
			ToAnimate.Add (moveOptions);
		}

		if (vis == true && mov == false) {
			mov = true;
			ToAnimate.Add (removeOptions);
		}
	}

	public void removeOptions()
	{
		GetComponent<RectTransform> ().localPosition = Vector3.MoveTowards (GetComponent<RectTransform> ().localPosition, og_pos, 100f);
		if (GetComponent<RectTransform> ().localPosition.x == og_pos.x) {
			vis = false;
			mov = false;
			ToAnimate.Remove (removeOptions);		
		}

	}
	//REVEALS THE OPTIONS
	public void moveOptions()
	{
		GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GetComponent<RectTransform>().localPosition, new Vector3(0,0,0), 100f);
		if (GetComponent<RectTransform> ().localPosition.x == 0) {
			vis = true;
			mov = false;
			ToAnimate.Remove (moveOptions);
		}
	}
		
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < ToAnimate.Count; i++)
			ToAnimate [i] ();
	}
}
