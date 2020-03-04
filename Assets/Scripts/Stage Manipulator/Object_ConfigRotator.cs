using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ConfigRotator : MonoBehaviour {

	public GameObject[] targets;
	public int configs;
	public float speed;
	private List<Quaternion[]> orientations = new List<Quaternion[]>(); 
	private int[] positionCount;
	// Use this for initialization

	void Awake()
	{
		positionCount = new int[targets.Length];
		for (int i = 0; i < positionCount.Length; i++)
			positionCount [i] = 0;
	}
	void Start () 
	{
		//FOR EACH TARGET
		for (int i = 0; i < targets.Length; i++) 
		{
			
			Quaternion[] targetRotations = new Quaternion[configs];

			for (int j = 0; j < targetRotations.Length; j++) {
				targetRotations [j] = new Quaternion (Random.Range (-360, 360), Random.Range (-360, 360), Random.Range (-360, 360), 0);
			}

			orientations.Add (targetRotations);
		}
		
	}

	public int cycle(int i)
	{
		if (i + 1 <= configs-1) {
			return i + 1;
		} else
			return 0;
	}

	// Update is called once per frame
	void FixedUpdate () {

		for (int i = 0; i < targets.Length; i++) 
		{
			targets [i].transform.rotation = Quaternion.Lerp (targets [i].transform.rotation, orientations[i][positionCount[i]], speed*Time.deltaTime / 1000*FindObjectOfType<itemBank>().getMoveSpeed());
			//Debug.Log (Vector3.Distance (targets [i].transform.rotation.eulerAngles, orientations [i] [positionCount [i]].eulerAngles));
			//Debug.Log (positionCount [i]);
			if (Vector3.Distance(targets [i].transform.rotation.eulerAngles,  orientations [i] [positionCount [i]].eulerAngles) < 1) {
				//Debug.Log ("HERE");
				positionCount [i] = cycle (positionCount[i]);
			}
		}
	}
}
