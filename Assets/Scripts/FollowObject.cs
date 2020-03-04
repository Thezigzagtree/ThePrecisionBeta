using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;

public class FollowObject : MonoBehaviour {

	public Agent agent;
	private List <System.Action> ToAnimate = new List<System.Action>();
	void Awake () {
		init_CenterToAgent ();
		agent.OnMoveStart += camFollowCor;
		agent.OnMoveStart += adjustSpeed;
		agent.OnMoveEnd += camFollowEnd;
	}

	void FollowAgent()
	{
		transform.position = Vector3.MoveTowards (transform.position, new Vector3(agent.transform.position.x-17.5f, transform.position.y, agent.transform.position.z-17.5f), 10);	
	}

	void adjustSpeed (Node targetNode)
	{
		agent.movementSpeed = agent.GetRoutePointPositions(targetNode).Count*5;
	}

	// Update is called once per frame
	void camFollowCor (Node targetNode) 
	{
		ToAnimate.Add (FollowAgent);			
			
	}

	void camFollowEnd (Node targetNode)
	{
		ToAnimate.Remove (FollowAgent);
	}

	public void init_CenterToAgent()
	{
		ToAnimate.Add (centerToAgent);

	}

	public void centerToAgent()
	{
		transform.position = Vector3.Lerp (transform.position, new Vector3(agent.transform.position.x-17.5f, transform.position.y, agent.transform.position.z-17.5f), 15*Time.deltaTime);
		if (transform.position == new Vector3(agent.transform.position.x-17.5f, transform.position.y, agent.transform.position.z-17.5f))
		{
			ToAnimate.Remove (centerToAgent);
		}
	}

	void Update ()
	{
		for (int i = 0; i < ToAnimate.Count; i++)
			ToAnimate [i] ();
	}


}
