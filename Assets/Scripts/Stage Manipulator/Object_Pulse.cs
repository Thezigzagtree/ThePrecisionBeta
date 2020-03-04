using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Pulse : MonoBehaviour {
	private List<System.Action> toAnimate = new List<System.Action>();
	public GameObject[] targets;
	public float shrinkFactor;
	public float shrinkSpeed;
	public float expandSpeed;
	private bool[] shrunk;
	private Vector3[] ogScales;
	// Use this for initialization

	void Awake()
	{
		shrunk = new bool[targets.Length];
		ogScales = new Vector3[targets.Length];
	}
	void Start () {


		//Initialize Shrunk and OGScales;
		for (int i = 0; i < targets.Length; i++) 
		{
			ogScales [i] = targets [i].transform.localScale;
			shrunk [i] = false;
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		for (int i = 0; i < targets.Length; i++) {
			if (shrunk [i] == true) {
				targets [i].transform.localScale = Vector3.MoveTowards (targets [i].transform.localScale, ogScales [i], Time.fixedDeltaTime*expandSpeed*FindObjectOfType<itemBank>().getMoveSpeed());
				if (targets [i].transform.localScale == ogScales [i])
					shrunk [i] = false;

			} else if (shrunk [i] == false) {
				targets [i].transform.localScale = Vector3.Lerp (targets [i].transform.localScale, new Vector3 (ogScales [i].x / shrinkFactor, ogScales [i].y / shrinkFactor, ogScales [i].z / shrinkFactor), Time.fixedDeltaTime * shrinkSpeed*FindObjectOfType<itemBank>().getMoveSpeed());
				if (targets [i].transform.localScale == new Vector3 (ogScales [i].x / shrinkFactor, ogScales [i].y / shrinkFactor, ogScales [i].z / shrinkFactor))
					shrunk [i] = true;
			}
		}
		
	}
}
