using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;

public class shrinkOnMove : MonoBehaviour {

	private Agent agent;
	private Vector3 og_size;
	public GameObject baseImage;
	private List<System.Action> toAnimate = new List<System.Action> ();
	void Awake()
	{
		og_size = baseImage.transform.localScale;
		agent = GameObject.FindGameObjectWithTag("Player").GetComponent<Agent>();

		agent.OnMoveStart += shrinkCircle;
		agent.OnMoveEnd += expandCircle;

	}

	public void shrinkSize()
	{
		baseImage.transform.localScale = Vector3.MoveTowards (baseImage.transform.localScale, new Vector3 (0.02f, 0.02f, 0.02f), Time.deltaTime);
	}

	public void expandSize()
	{
		baseImage.transform.localScale = Vector3.MoveTowards (baseImage.transform.localScale, og_size, Time.deltaTime);
		if (Vector3.Distance (baseImage.transform.localScale, og_size) < 0.01) {
			toAnimate.Remove (expandSize);
		}
	}

	public void shrinkCircle(Node targetNode)
	{
		toAnimate.Add (shrinkSize);
	}

	public void expandCircle(Node targetNode)
	{
		toAnimate.Remove (shrinkSize);
		toAnimate.Add (expandSize);

	}

	void Update()
	{
		for (int i = 0; i < toAnimate.Count; i++) {
			toAnimate [i] ();
		}	
	}
}
