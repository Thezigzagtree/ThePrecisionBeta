using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;

public class SceneryStarLock : MonoBehaviour {

		public int Lock;

		public void showLock()
		{
			GameObject starLock = Instantiate (Resources.Load("Other/SceneryStarLock")) as GameObject;
			starLock.transform.position = new Vector3 (transform.parent.position.x, transform.parent.position.y+10, transform.parent.position.z);
			starLock.transform.SetParent (transform.parent);
		//starLock.transform.localPosition = new Vector3 (transform.localPosition.x, 1, transform.localPosition.z);
			starLock.GetComponentInChildren<TextMesh> ().text = Lock.ToString();
		}

	void Start()
	{
		if (FindObjectOfType<WorldMapManager>().starCount < Lock) 
		{
			for (int i = 0; i < transform.parent.GetComponents<MonoBehaviour> ().Length; i++)
				transform.parent.GetComponents<MonoBehaviour> ()[i].enabled = false;
			for (int i = 0; i < transform.parent.GetComponents<MeshRenderer> ().Length; i++)
				transform.parent.GetComponents<MeshRenderer> ()[i].sharedMaterial = FindObjectOfType<sceneryManager> ().disableMat;
			showLock ();
		}


	}

	}

