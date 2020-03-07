using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapLine : MonoBehaviour {

	public List<GameObject> endPoints;

	private LineRenderer line;


	void Start()
	{
		for (int i = 0; i < endPoints.Count; i++) {
			if (SaveSystem.stageToRank.ContainsKey(endPoints[i].name) &&  SaveSystem.stageToRank[endPoints[i].name] > 0)
			//if (FindObjectOfType<playerObj>().stageToRank.ContainsKey(endPoints[i].name) && FindObjectOfType<playerObj>().stageToRank[endPoints[i].name] > 0) 
			{
				
				endPoints.Remove (endPoints [i]);
				i -= 1;
			}
		}

		if (endPoints.Count > 0) 
		{
			for (int i = 0; i < transform.parent.GetComponents<MonoBehaviour> ().Length; i++)
				transform.parent.GetComponents<MonoBehaviour> ()[i].enabled = false;
			for (int i = 0; i < transform.parent.GetComponents<MeshRenderer> ().Length; i++)
				transform.parent.GetComponents<MeshRenderer> ()[i].sharedMaterial = FindObjectOfType<sceneryManager> ().disableMat;
			
			line = this.gameObject.AddComponent<LineRenderer> ();
			line.useWorldSpace = true;
			line.sharedMaterial = FindObjectOfType<sceneryManager> ().lineMat;
			line.startColor = new Color (0.8f, 0, 0, 0.05f);
			line.endColor = new Color (0.8f, 0.8f, 0, 1);

		}


	}


	void Update()
	{
		for (int i = 0; i < endPoints.Count*2; i++) 
		{
			line.startWidth = 0.55f;
			line.endWidth = 0.55f;
			line.positionCount =  endPoints.Count * 2;

			if (i % 2 == 0) 
			{
				line.SetPosition (i, transform.parent.position);

			} 
			else {line.SetPosition (i, new Vector3(endPoints [i/2].transform.position.x,endPoints [i/2].transform.position.y-0.1f,endPoints [i/2].transform.position.z));
				

			}

		}
	}
}
	

