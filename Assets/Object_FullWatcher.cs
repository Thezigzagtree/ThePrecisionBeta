using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_FullWatcher : MonoBehaviour {

		public GameObject StageTargets;
		public GameObject center;
	private GameObject[] targets;


		void Update()
		{
			for (int i = 0; i < targets.Length; i++)
			{
				targets [i].transform.rotation = Quaternion.LookRotation (Vector3.RotateTowards (targets [i].transform.position, center.transform.position, Time.deltaTime, Time.deltaTime));
			}
		}



	// Use this for initialization
	void Start () {
		targets = new GameObject[StageTargets.transform.childCount];
		for (int i = 0; i < StageTargets.transform.childCount; i++) 
		{
			targets [i] = StageTargets.transform.GetChild (i).gameObject;	
		}	


	}
	
}
