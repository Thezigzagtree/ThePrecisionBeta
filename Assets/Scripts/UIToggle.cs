using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;

public class UIToggle : MonoBehaviour {

	public Agent agent;
	private Vector3 og_pos;
	private List <System.Action> ToAnimate = new List<System.Action>();
	void Awake () {
		og_pos = GetComponent<RectTransform>().localPosition;

		agent.OnMoveStart += hideUI;
		agent.OnMoveEnd += showUI;
	}


	public void removeUI()
	{
		GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GetComponent<RectTransform>().localPosition, new Vector3(GetComponent<RectTransform>().localPosition.x, og_pos.y-200, 0), 25f);

	}

	public void returnUI()
	{
		GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GetComponent<RectTransform>().localPosition, og_pos, 25f);
		if (GetComponent<RectTransform> ().localPosition.y == og_pos.y) 
		{
			ToAnimate.Remove (returnUI);
		}

		//ShowCorrectHit(new Vector3 (540, 960, 0));
	}

	// Use this for initialization
	void hideUI (Node targetNode) {
		ToAnimate.Add (removeUI);
	}

	// Update is called once per frame
	void showUI (Node targetNode) {
		ToAnimate.Remove (removeUI);
		ToAnimate.Add (returnUI);
	}

	void Update()
	{	
		for (int i = 0; i < ToAnimate.Count; i++) {
			ToAnimate [i] ();
		}
	}
}
