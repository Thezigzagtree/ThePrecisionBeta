using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_railShooter : MonoBehaviour {

	public GameObject[] targets;
	private Vector3[] ogPositions;
	private bool[] reset;
	public Vector3 destination;
	public float speed;
	// Use this for initialization

	void Awake ()
	{
		ogPositions = new Vector3[targets.Length];
		reset = new bool[targets.Length];
		for (int i = 0; i < targets.Length; i++) {
			reset [i] = false;
			ogPositions [i] = targets [i].transform.position;
		}
	}
		
	// Update is called once per frame
	void FixedUpdate () 
	{

		for (int i = 0; i < targets.Length; i++) 
		{
			if (reset [i] == false)
			{
				targets [i].transform.position = Vector3.MoveTowards (targets [i].transform.position, destination, speed * Time.deltaTime * FindObjectOfType<itemBank> ().getMoveSpeed ());
				if (targets [i].transform.position == destination)
					reset [i] = true;
			} else {
				targets [i].transform.position = Vector3.Lerp (targets [i].transform.position, ogPositions [i], speed * Time.deltaTime * FindObjectOfType<itemBank> ().getMoveSpeed ());
				if (targets [i].transform.position == ogPositions [i])
					reset [i] = false;
			}

		}
		
	}
}
