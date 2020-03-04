using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_PlaceRotatorRandom: MonoBehaviour {

	public GameObject[] targets;
	private float speed;
	private Vector3 Rotation;
	private float switchTime;

	public void setSpeed(float s)
	{
		speed = s;
	}


	public float getSpeed()
	{
		return speed;
	}


	public void changeRotation()
	{
		switchTime = Time.timeSinceLevelLoad + Random.Range(0.1f, 2f);
		speed = Random.Range (2.5f, 6f);
		Rotation = new Vector3 (Random.Range(-15, 15),Random.Range(-10, 10),Random.Range(-15, 15));

	}

	void Awake ()
	{
		changeRotation ();
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		for (int i = 0; i < targets.Length;i++)
			targets[i].transform.Rotate (Rotation*(speed*FindObjectOfType<itemBank>().getMoveSpeed()*Time.deltaTime));

		if (Time.timeSinceLevelLoad - switchTime > 4)
			changeRotation ();
	}
}
