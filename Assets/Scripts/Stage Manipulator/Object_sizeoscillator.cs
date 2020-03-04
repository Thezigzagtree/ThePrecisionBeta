using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_sizeoscillator : MonoBehaviour {

	public GameObject[] targets;
	public float speed;
	private Vector3[] OG_sizes;

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
		OG_sizes = new Vector3[targets.Length];

		for (int i = 0; i < OG_sizes.Length; i++) {
			OG_sizes [i] = targets [i].transform.localScale;
		}

	}

	void FixedUpdate()
	{
		for (int i = 0; i < targets.Length; i++) {
			targets [i].transform.localScale = new Vector3 (OG_sizes [i].x * Mathf.Abs(Mathf.Cos (Time.timeSinceLevelLoad * FindObjectOfType<itemBank> ().getMoveSpeed () * speed)), OG_sizes [i].y * Mathf.Abs(Mathf.Cos (Time.timeSinceLevelLoad * FindObjectOfType<itemBank> ().getMoveSpeed () * speed)), OG_sizes [i].z * Mathf.Abs(Mathf.Cos (Time.timeSinceLevelLoad * FindObjectOfType<itemBank> ().getMoveSpeed () * speed)));
		}
	}


}
