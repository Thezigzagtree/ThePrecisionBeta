using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_depthmovement : MonoBehaviour {

	public GameObject[] targets;
	public float DepthLimit;
	public float DepthMin;

	public float speed;

	private Vector3[] CurrentDestination;

	void Awake()
	{

		CurrentDestination = new Vector3[targets.Length];

		for (int i = 0; i < targets.Length; i++) 
		{
			CurrentDestination [i] = new Vector3 (targets[i].transform.position.x, targets [i].transform.position.y, DepthLimit);
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			targets [i].transform.position = Vector3.MoveTowards (targets [i].transform.position, CurrentDestination[i], speed*Time.deltaTime*FindObjectOfType<itemBank>().getMoveSpeed());
			//new Vector3 ( range * Mathf.Cos (speed * Time.timeSinceLevelLoad)*Time.deltaTime, targets[i].transform.position.y, targets [i].transform.position.z);

		}
	}

	void LateUpdate()
	{
		for (int i = 0; i < targets.Length; i++) 
		{

			if (Vector3.Distance (targets [i].transform.position, new Vector3 (targets[i].transform.position.x, targets[i].transform.position.y, DepthMin)) < 0.1f)
				CurrentDestination [i] = new Vector3 (targets[i].transform.position.x, CurrentDestination [i].y, DepthLimit);

			else if (Vector3.Distance (targets[i].transform.position, new Vector3 (targets[i].transform.position.x, targets[i].transform.position.y, DepthLimit)) < 0.1f)
				CurrentDestination [i] = new Vector3 (CurrentDestination[i].x, CurrentDestination[i].y, DepthMin);
		}

	}
}