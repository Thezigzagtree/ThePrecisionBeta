using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;

public class starLock : MonoBehaviour {

	public int Lock;
		
	public void showLock()
	{
		GameObject starLock = Instantiate (Resources.Load("Other/StarLock")) as GameObject;
		starLock.transform.position = new Vector3 (transform.localPosition.x, transform.localPosition.y + 2.5f, transform.localPosition.z);
		starLock.transform.SetParent (transform);
		starLock.GetComponentInChildren<TextMesh> ().text = Lock.ToString();
	}


}
	