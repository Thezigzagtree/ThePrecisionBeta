using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Object_verticalmovement : MonoBehaviour {

	public GameObject[] targets;
	public float VerticalLimit;
	public float Verticalmin;

	public float speed;
	private Vector3 DampVelocity;

	private Vector3[] CurrentDestination;

	public void setSpeed(float s)
	{
		speed = s;
	}


	public float getSpeed()
	{
		return speed;
	}

	void Awake()
	{

		CurrentDestination = new Vector3[targets.Length];

		for (int i = 0; i < targets.Length; i++) 
		{
			CurrentDestination [i] = new Vector3 (targets[i].transform.position.x, VerticalLimit, targets [i].transform.position.z);
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

			if (Vector3.Distance (targets [i].transform.position, new Vector3 (targets[i].transform.position.x, VerticalLimit, targets [i].transform.position.z)) < 0.1f)
				CurrentDestination [i] = new Vector3 (CurrentDestination [i].x, Verticalmin, CurrentDestination [i].z);

			else if (Vector3.Distance (targets[i].transform.position, new Vector3 (targets[i].transform.position.x, Verticalmin, targets [i].transform.position.z)) < 0.1f)
				CurrentDestination [i] = new Vector3 (CurrentDestination [i].x, VerticalLimit, CurrentDestination [i].z);
			
		}

	}
}
