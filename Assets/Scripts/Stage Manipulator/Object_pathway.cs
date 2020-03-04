using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_pathway : MonoBehaviour {

	public GameObject target;
	public Vector3[] destinations;
	public float SmoothTime;

	private Vector3[] positions;
	private Vector3 DampVelocity;
	private int count;

		
	void Awake()
	{
		count = 0;
		positions = new Vector3[destinations.Length+1];
		positions[0] = target.transform.position;
		//positions [0] = targets [i];
	}

	// Use this for initialization
	void Start () 
	{
		for (int i = 1; i < positions.Length; i++) {
			positions [i] = destinations [i-1];
		}
			
	}

	public void PathwayMovement()
	{
		target.transform.position = Vector3.SmoothDamp (target.transform.position, positions[count], ref DampVelocity, SmoothTime/FindObjectOfType<itemBank>().getMoveSpeed());

		if (target.transform.position == positions [count]) 
		{
			if (count < positions.Length-1)
				count += 1;
			else
				count = 0;
		}	
	}

	// Update is called once per frame
	void FixedUpdate () {

		PathwayMovement ();
			
	}
}
