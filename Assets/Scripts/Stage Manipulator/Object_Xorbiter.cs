﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Xorbiter : MonoBehaviour {

	public GameObject[] targets;
	public GameObject center;
	private Vector3 rotate_axis;

	public float speed;

	GameObject rotator;



	public void calculate_centeraxis()
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			rotate_axis += targets [i].transform.position;
		}
		rotate_axis = new Vector3 (rotate_axis.x / targets.Length, rotate_axis.y / targets.Length, 0);

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
		//rotator.transform.SetParent (GameObject.FindGameObjectWithTag("StageTargets").transform);
		rotator.transform.SetParent (center.transform);
	}

	void FixedUpdate()
	{
		rotator.transform.localPosition = center.transform.localPosition;
		rotator.transform.Rotate (new Vector3 (0, 0, 10)*(FindObjectOfType<itemBank>().getMoveSpeed()*speed*Time.deltaTime));

	}

}
