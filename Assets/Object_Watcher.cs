using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Watcher : MonoBehaviour {

	public GameObject[] targets;
	public GameObject center;

	void Update()
	{
		for (int i = 0; i < targets.Length; i++)
		{
			targets [i].transform.rotation = Quaternion.LookRotation (Vector3.RotateTowards (targets [i].transform.position, center.transform.position, Time.deltaTime, Time.deltaTime));
		}
	}
}
