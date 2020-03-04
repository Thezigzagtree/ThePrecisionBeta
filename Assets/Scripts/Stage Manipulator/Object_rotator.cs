using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_rotator : MonoBehaviour {

	public GameObject[] targets;
	private Vector3 rotate_axis;
	public float speed;

	GameObject rotator;

	public void calculate_centeraxis()
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			rotate_axis += targets [i].transform.localPosition;
		}
		rotate_axis.x = (rotate_axis.x / targets.Length);
		rotate_axis.y = (rotate_axis.y / targets.Length);

	}

	public void add_objects_to_rotator()
	{	
		for (int i = 0; i < targets.Length; i++) 
		{
			targets [i].transform.parent = rotator.transform;
		}
	}

	void Start()
	{
		calculate_centeraxis ();
		rotator = new GameObject ("Rotator");
		rotator.transform.localPosition = rotate_axis;
		add_objects_to_rotator ();
		rotator.transform.SetParent (transform.parent);
	}

	void FixedUpdate()
	{
		rotator.transform.Rotate (new Vector3 (0, 0, 10)*(FindObjectOfType<itemBank>().getMoveSpeed()*speed*Time.deltaTime));

	}

}
