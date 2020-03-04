using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Dodger : MonoBehaviour {


	//MOVES RANDOMLY OFFSET THEN GOES BACK TO ORIGINAL POSITION
	public GameObject[] targets;
	public float speed;
	public float offSet;


	private Vector3[] ogPos;
	private Vector3[] destinations;
	private bool[] comingBack;

	public void newDestination(int i)
	{
		destinations [i] = new Vector3 (ogPos [i].x + Random.Range (-offSet, offSet), 
			ogPos [i].y + Random.Range (-offSet, offSet),
			ogPos [i].z+ Random.Range(-offSet, offSet));
		
	}

	void Awake() {
		ogPos = new Vector3[targets.Length];
		comingBack = new bool[targets.Length];
		destinations = new Vector3[targets.Length];

		for (int i = 0; i < targets.Length; i++) {
			ogPos [i] = targets [i].transform.position;
			comingBack [i] = false;
			newDestination (i);
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{

		for (int i = 0; i < targets.Length; i++) {


			if (comingBack [i]) {

				targets [i].transform.position = Vector3.Lerp (targets [i].transform.position, ogPos [i], Time.deltaTime * FindObjectOfType<itemBank> ().getMoveSpeed ()*speed);
				if (targets [i].transform.position == ogPos [i]) {
					comingBack [i] = false;
				}
			} 


			else {
				targets [i].transform.position = Vector3.Lerp (targets [i].transform.position, destinations [i], Time.deltaTime * FindObjectOfType<itemBank> ().getMoveSpeed ()*speed);
				if (targets [i].transform.position == destinations [i]) {
					comingBack [i] = true;
					newDestination (i);
				}

				
			}


		}
		
	}
}
