using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;

public class stageLock : MonoBehaviour {

	public List<GameObject> reqs;
	public List<GameObject> bridgeTo;
	private LineRenderer line;
	private GameObject lockSymbol;
	private List<System.Action> toAnimate = new List<System.Action> ();

	void Awake()
	{
		showLock ();
	}

	public void showLock()
	{
		
		for (int i = 0; i < reqs.Count; i++) {
			
			if(SaveSystem.GetInt(reqs[i].name) > 0)
			
			//if (FindObjectOfType<playerObj>().stageToRank.ContainsKey(reqs[i].name) && FindObjectOfType<playerObj>().stageToRank[reqs[i].name] > 0) 
			{
				reqs.Remove (reqs [i]);

				i -= 1;
			}
		}


		if (reqs.Count > 0) 
		{
			lockSymbol = Instantiate (Resources.Load("Other/stageLock")) as GameObject;
			lockSymbol.transform.position = new Vector3 (transform.localPosition.x, transform.localPosition.y + 8.5f, transform.localPosition.z);
			lockSymbol.transform.SetParent (transform);
			line = lockSymbol.AddComponent<LineRenderer> ();
			line.useWorldSpace = true;
			line.sharedMaterial = FindObjectOfType<sceneryManager> ().stageLockMat;
			line.startColor = new Color (0.8f, 0, 0, 0.05f);
			line.endColor = new Color (0.8f, 0.8f, 0, 1);
			toAnimate.Add (drawLines);


		} 
		else {
			buildBridges ();
		}

	}

	public void drawLines()
	{
		for (int i = 0; i < reqs.Count*2; i++) 
		{
			line.startWidth = 0.55f;
			line.endWidth = 0.55f;
			line.positionCount =  reqs.Count * 2;

			if (i % 2 == 0) 
			{
				line.SetPosition (i, lockSymbol.transform.position);

			} 
			else {line.SetPosition (i, new Vector3( reqs [i/2].transform.position.x,  reqs [i/2].transform.position.y+0.1f,  reqs [i/2].transform.position.z));


			}

		}
	}

	public void buildBridges()
	{
		foreach (GameObject loc in bridgeTo) 
		{
			transform.GetComponent<Node> ().CreatePath (loc.GetComponent<Node>(), true);
			for(int i = 0; i < transform.GetComponent<Node>().GetAllPaths().Length; i++)
			{
				transform.GetComponent<Node> ().GetAllPaths () [i].GetComponent<Path> ().drawMarkers = false;
				transform.GetComponent<Node> ().GetAllPaths () [i].GetComponent<Path> ().drawLine = true;
				transform.GetComponent<Node> ().GetAllPaths () [i].pathDirection = MovementType.TwoWay;
				transform.GetComponent<Node> ().GetAllPaths () [i].GetComponent<LineRenderer> ().sharedMaterial = FindObjectOfType<WorldMapManager>().AccessibleMat;
				FindObjectOfType<Map> ().RedrawMap (true);
				FindObjectOfType<Map> ().Refresh ();

			}
		}

	}

	void Update()
	{
		for (int i = 0; i < toAnimate.Count; i++) {
			toAnimate [i] ();

		}

	}

}
