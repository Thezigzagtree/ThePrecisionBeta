using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_WorldMapFlatRotator : MonoBehaviour 
{
	public float speed;
	public void setSpeed(float s)
	{
		speed = s;
	}

	public float getSpeed()
	{
		return speed;
	}
	// Update is called once per frame
	void FixedUpdate () 
	{
			transform.Rotate (new Vector3(0,0,1)*(speed*Time.deltaTime));
	}
}
