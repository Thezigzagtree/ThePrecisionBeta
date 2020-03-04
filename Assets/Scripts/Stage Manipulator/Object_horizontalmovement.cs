using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Object_horizontalmovement : MonoBehaviour {

	public GameObject[] targets;
	public float HorizontalLimit;
	public float Horizontalmin;

	public float speed;

	private Vector3[] CurrentDestination;

	void Awake()
	{
		
		CurrentDestination = new Vector3[targets.Length];

		for (int i = 0; i < targets.Length; i++) 
		{
			CurrentDestination [i] = new Vector3 (HorizontalLimit, targets [i].transform.position.y, targets [i].transform.position.z);
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

			if (Vector3.Distance (targets [i].transform.position, new Vector3 (HorizontalLimit, targets[i].transform.position.y, targets [i].transform.position.z)) < 0.1f)
				CurrentDestination [i] = new Vector3 (Horizontalmin, CurrentDestination [i].y, CurrentDestination [i].z);

			else if (Vector3.Distance (targets[i].transform.position, new Vector3 (Horizontalmin, targets[i].transform.position.y, targets [i].transform.position.z)) < 0.1f)
				CurrentDestination [i] = new Vector3 (HorizontalLimit, CurrentDestination[i].y, CurrentDestination [i].z);
			}
			
	}
}
