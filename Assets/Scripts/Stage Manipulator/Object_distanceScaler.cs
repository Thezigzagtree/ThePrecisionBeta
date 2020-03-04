using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_distanceScaler : MonoBehaviour {

	public GameObject[] targets;
	public float speed;
	private Vector3[] ogScales;
	public GameObject center;
	public Vector3 bigConfig;
	public Vector3 smallConfig;
	private Vector3 midConfig;
	public float maxDist;
	public float minDist;
	private float midDist;
	public bool shrinkWhenAway;


	void Awake()
	{
		ogScales = new Vector3[targets.Length];
		for (int i = 0; i < targets.Length; i++)
		{
			ogScales [i] = targets [i].transform.localScale;
		}

		setupMidValues ();
		
	}

	public void setupMidValues()
	{
		midDist = (maxDist + minDist) / 2;
		midConfig = (bigConfig + smallConfig) / 2;
	}

	public Vector3 calculateScale(GameObject target)
	{
		if (shrinkWhenAway) {
			if (Vector3.Distance (target.transform.position, center.transform.position) >= (midDist * 1.5f)) {
				return smallConfig;
			} else if (Vector3.Distance (target.transform.position, center.transform.position) <= (midDist * 1.5f) && Vector3.Distance (target.transform.position, center.transform.position) >= (midDist * 0.5f))
			{
				return midConfig;
				
			} else
				return bigConfig;

		} 

		else {
			if (Vector3.Distance (target.transform.position, center.transform.position) >= midDist * 1.5f) {
				return bigConfig;
			} else if (Vector3.Distance (target.transform.position, center.transform.position) <= (midDist * 1.5f) && Vector3.Distance (target.transform.position, center.transform.position) >= (midDist * 0.5f)) {
				return midConfig;

			} else
				return smallConfig;
			
		}
	}

	void Update()
	{
		for (int i = 0; i < targets.Length; i++) {
			targets [i].transform.localScale = Vector3.Lerp(targets[i].transform.localScale, calculateScale (targets [i]), Time.deltaTime*FindObjectOfType<itemBank>().getMoveSpeed()*speed);
		}
	}
}
