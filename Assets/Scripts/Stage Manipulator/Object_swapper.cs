using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_swapper : MonoBehaviour {

	public GameObject[] targets;
	public float speed = 1;
	private Vector3[] destinations;
	private bool targets_reached = false;
	private float moveSpeed;



	void Awake()
	{
		destinations = new Vector3[targets.Length];
		setup_destinations ();
	}
	public void setup_destinations()
	{
		for (int i = 0; i < targets.Length-1; i++) 
		{
			destinations [i] = targets [i + 1].transform.localPosition;
		}

		destinations [targets.Length-1] = targets [0].transform.localPosition;

	}

	public void check_destinations()
	{
		
		for (int i = 0; i < targets.Length; i++) 
		{
			if (targets [i].transform.localPosition == destinations [i]) 
			{
			} else
				return;

			if (i == targets.Length - 1)
				targets_reached = true;
		}
	}




	public void swap_targets()
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			targets [i].transform.localPosition = Vector3.Lerp (targets [i].transform.localPosition, destinations [i], Time.deltaTime * speed*FindObjectOfType<itemBank>().getMoveSpeed());
			if (targets_reached) 
			{
				setup_destinations ();
				targets_reached = false;
			}
		}	
	}


	void FixedUpdate()
	{
		swap_targets ();
		check_destinations ();
	}
}
