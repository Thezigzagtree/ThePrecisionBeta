using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Zrotator : MonoBehaviour {

	public GameObject[] targets;
	private Vector3 rotate_axis;
	public float speed;

	public void setSpeed(float s)
	{
		speed = s;
	}


	public float getSpeed()
	{
		return speed;
	}

	GameObject rotator;

	public void calculate_centeraxis()
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			rotate_axis += targets [i].transform.position;
		}
		rotate_axis.x = (rotate_axis.x / targets.Length);
		rotate_axis.z = (rotate_axis.z / targets.Length);

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
		rotator.transform.position = rotate_axis;
		add_objects_to_rotator ();
		rotator.transform.SetParent (transform.parent);

	}

	void FixedUpdate()
	{
		rotator.transform.Rotate (new Vector3 (0, 10, 0)*(FindObjectOfType<itemBank>().getMoveSpeed()*speed*Time.deltaTime));

	}

}
