using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ShapeMorpher : MonoBehaviour {

	public GameObject[] targets;
	public GameObject[] mutatorShapes;
	private Vector3[] ogScales;
	public float speed;

	void Awake()
	{
		ogScales = new Vector3[targets.Length];

		for (int i = 0; i < ogScales.Length; i++) {
			ogScales [i] = targets [i].transform.localScale;
		}

	}

	public void changeShape(GameObject target)
	{
		target.GetComponent<MeshFilter> ().mesh = mutatorShapes [Random.Range (0, mutatorShapes.Length)].GetComponent<MeshFilter>().sharedMesh;
		

		
	}
	// Update is called once per frame
	void FixedUpdate()
	{
		for (int i = 0; i < targets.Length; i++) {
			targets [i].transform.localScale = new Vector3 (ogScales [i].x * Mathf.Abs(Mathf.Cos (Time.timeSinceLevelLoad * FindObjectOfType<itemBank> ().getMoveSpeed () * speed)), ogScales [i].y * Mathf.Abs(Mathf.Cos (Time.timeSinceLevelLoad * FindObjectOfType<itemBank> ().getMoveSpeed () * speed)), ogScales [i].z * Mathf.Abs(Mathf.Cos (Time.timeSinceLevelLoad * FindObjectOfType<itemBank> ().getMoveSpeed () * speed)));
			if (targets [i].transform.localScale.x < 0.05f)
				changeShape (targets [i]);
		}
		
	}

}
