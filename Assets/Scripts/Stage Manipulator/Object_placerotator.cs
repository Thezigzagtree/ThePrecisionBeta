using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_placerotator : MonoBehaviour {

	public GameObject[] targets;
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
		for (int i = 0; i < targets.Length; i++) 
		{
			targets[i].transform.Rotate (new Vector3(0,10,0)*(speed*Time.deltaTime*FindObjectOfType<itemBank>().getMoveSpeed()));
		}
	}
}
