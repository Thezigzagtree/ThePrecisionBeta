using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_RoamerTow : MonoBehaviour {


	public GameObject[] targets;
	public Vector2 xStat;
	public Vector2 yStat;
	public float speed;
	private Vector3[] destinations;

	void Awake () 
	{
		destinations = new Vector3[targets.Length];
	}
		
	public void setupDestination(int i)
	{
		destinations [i] = new Vector3 (Random.Range (xStat[0], xStat[1]), Random.Range (yStat[0], yStat[1]), targets [i].transform.position.z);

	}
	// Use this for initialization
	void Start () {

		for (int i = 0; i < targets.Length; i++)
			setupDestination (i); 

	}

	void Update()
	{
		for (int i = 0; i < targets.Length; i++) {

			if (targets [i].transform.position == destinations [i])
				setupDestination (i);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		for (int i = 0; i < targets.Length; i++) {
			targets [i].transform.position = Vector3.MoveTowards (targets [i].transform.position, destinations [i], Time.deltaTime*FindObjectOfType<itemBank> ().getMoveSpeed ()*speed);
			}


	}
}
