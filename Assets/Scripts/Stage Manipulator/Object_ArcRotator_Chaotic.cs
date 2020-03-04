using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ArcRotator_Chaotic : MonoBehaviour {

		public GameObject[] targets;
		private Vector3 rotate_axis;
	public float speed;
	private float switchspeed;
		//private enum {"Forward", "Backward"};
	public void setSpeed(float s)
	{
		speed = s;
	}

		GameObject rotator;

	public void calculate_centeraxis()
		{
			for (int i = 0; i < targets.Length; i++) 
			{
				rotate_axis += targets [i].transform.position;
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
			switchspeed = Time.timeSinceLevelLoad + Random.Range (2f, 5f);
			calculate_centeraxis ();
			rotator = new GameObject ("Rotator");
			rotator.transform.position = rotate_axis;
			add_objects_to_rotator ();
			rotator.transform.SetParent (GameObject.FindGameObjectWithTag("StageTargets").transform);

		}

		void FixedUpdate()
		{
		rotator.transform.Rotate (new Vector3 (0, 0, 10)*(speed*Time.deltaTime*FindObjectOfType<itemBank>().getMoveSpeed()));

		if (Time.timeSinceLevelLoad > switchspeed)
				//if (Time.timeSinceLevelLoad > switchspeed)
			{
			switchspeed = Time.timeSinceLevelLoad + Random.Range(0.5f, 5f);
			speed = -speed;

			}

		}

	}