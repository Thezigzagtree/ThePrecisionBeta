using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;

public class CheckValidPaths : MonoBehaviour {
	public Agent agent;
	public GameObject fullMap;
	void Awake () 
	{
		agent.OnNodeArrive += pathCheck;
	}
	// Use this for initialization
	void pathCheck (Node reachedNode, bool isTargetNode)
	{
		if (!PlayerPrefs.HasKey(reachedNode.name))
		{
			for (int i = 0; i < reachedNode.GetAllPaths().Length; i++) 
			{
				reachedNode.GetAllPaths () [i].pathDirection = MovementType.Impassable;
				//Debug.Log (reachedNode.GetAllPaths () [i].name);
			}
		}
	}


}
