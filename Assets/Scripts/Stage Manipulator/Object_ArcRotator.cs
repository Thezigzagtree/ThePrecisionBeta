using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ArcRotator : MonoBehaviour {
	
	public GameObject[] targets;
		private Vector3 rotate_axis;
		public float speed;
		public float Arc;
		private float arc_value;
		//private enum {"Forward", "Backward"};
		
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
		arc_value = Arc / 360;
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
			calculate_centeraxis ();
			rotator = new GameObject ("Rotator");
			rotator.transform.position = rotate_axis;
			add_objects_to_rotator ();
			rotator.transform.SetParent (GameObject.FindGameObjectWithTag ("StageCore").transform);
			//switchspeed = switchspeed+Time.timeSinceLevelLoad;

		}

		void FixedUpdate()
		{
		rotator.transform.Rotate (new Vector3 (0, 0, 10)*(speed*Time.deltaTime*FindObjectOfType<itemBank>().getMoveSpeed()));
		//Debug.Log (rotator.transform.rotation.z);
		if (Mathf.Abs(rotator.transform.rotation.z) >= arc_value)
		//if (Time.timeSinceLevelLoad > switchspeed)
		{
			

		//	switchspeed = switchspeed + Time.timeSinceLevelLoad;
			speed = -speed;
		}

		}

	}

