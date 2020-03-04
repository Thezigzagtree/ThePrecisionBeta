using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;

public class expandNodes : MonoBehaviour {

	private Agent agent; 

	private List<GameObject> nodesToExpand = new List<GameObject>();
	private List<Vector3> nodesToExpandOgScales = new List<Vector3>();
	private List<bool> gettingLarge = new List<bool>();

	private bool expanding;
	private Vector3 ogScale;
	private GameObject finalNode;


	void Awake()
	{
		agent = GameObject.FindGameObjectWithTag ("Player").GetComponent<Agent> ();
		agent.OnNodeArrive += addTarget;
	}



	public void expandTargetNodes(int num)
	{
		if (gettingLarge [num]) 
		{
			nodesToExpand [num].transform.localScale = Vector3.Lerp (nodesToExpand [num].transform.localScale, nodesToExpandOgScales [num], Time.deltaTime * 15);
			if (Vector3.Distance (nodesToExpand [num].transform.localScale, nodesToExpandOgScales [num]) <= 0.01f) {
				nodesToExpand.RemoveAt (num);
				nodesToExpandOgScales.RemoveAt (num);
				gettingLarge.RemoveAt (num);
				//Debug.Log ("FULL REMOVE");
			}
		} else {
			nodesToExpand [num].transform.localScale = Vector3.Lerp (nodesToExpand [num].transform.localScale, new Vector3 (nodesToExpandOgScales [num].x+1, nodesToExpandOgScales [num].y+1, nodesToExpandOgScales [num].z+1), 10 * Time.deltaTime);
			if (Vector3.Distance (nodesToExpand [num].transform.localScale, new Vector3 (nodesToExpandOgScales [num].x+1, nodesToExpandOgScales [num].y+1, nodesToExpandOgScales [num].z+1)) <= 0.01f) {
				gettingLarge [num] = true;
			}


		}
	}

	void addTarget(Node reachedNode, bool isTargetNode)
	{
		for (int i = 0; i < reachedNode.transform.childCount; i++) {

			if (reachedNode.transform.GetChild (i).tag == "Target" && !nodesToExpand.Contains(reachedNode.transform.GetChild(i).gameObject)) 
			{
				nodesToExpandOgScales.Add (reachedNode.transform.GetChild (i).transform.localScale);
				gettingLarge.Add (false);
				nodesToExpand.Add (reachedNode.transform.GetChild (i).gameObject);
			}
		}
		
	}

	void Update ()
	{
		for (int i = 0; i < nodesToExpand.Count; i++)
		{
			expandTargetNodes (i);
		}

	
}
}
