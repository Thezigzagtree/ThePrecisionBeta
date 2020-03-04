using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_circle_movement : MonoBehaviour {

	public GameObject[] targets;

//	public string direction;
	public float speed;
	public int range;

	private Vector3[] og_positions;

	public void setSpeed(float s)
	{
		speed = s;
	}

	// Use this for initialization
	void Awake()
	{
		og_positions = new Vector3[targets.Length];
	
	}
	void Start () {
		for (int i = 0; i < targets.Length; i++)
		{
			og_positions [i] = targets [i].transform.position;
			}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			targets [i].transform.position = new Vector3 (og_positions[i].x + range * Mathf.Sin (speed*FindObjectOfType<itemBank>().getMoveSpeed() * Time.timeSinceLevelLoad) * Time.deltaTime, og_positions[i].y + range * Mathf.Cos (speed*FindObjectOfType<itemBank>().getMoveSpeed() * Time.timeSinceLevelLoad) * Time.deltaTime, 
				og_positions[i].z);
		}

	}
}
