using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldMap_sizeoscillator : MonoBehaviour {

	private Vector3 ogScale;
	private bool shrunk;
	void Awake()
	{
		ogScale = transform.localScale;
		shrunk = false;
	}


		public float speed;
		// Update is called once per frame
		void Update () 
		{
		if (shrunk) 
		{
			transform.localScale = Vector3.Lerp (transform.localScale, ogScale, Time.deltaTime * speed);
			if (transform.localScale == ogScale)
				shrunk = false;
		}

		else
		{
			transform.localScale = Vector3.MoveTowards (transform.localScale, new Vector3(ogScale.x/2,ogScale.y/2,ogScale.z/2), Time.deltaTime * speed);
			if (transform.localScale == new Vector3(ogScale.x/2,ogScale.y/2,ogScale.z/2))
				shrunk = true;
			
		}


}
}
