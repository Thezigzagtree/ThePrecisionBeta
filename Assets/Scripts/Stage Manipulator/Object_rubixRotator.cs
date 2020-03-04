using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_rubixRotator : MonoBehaviour {

	public GameObject[] targets;
	public float speed;
	public bool xRot;
	public bool yRot;
	public bool zRot;

	private Quaternion[] rotationVectors;

	private int rotationCount;

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
		rotationVectors = new Quaternion[targets.Length];
	}


	public void setRotationVector()
	{
		int tempx;
		int tempy;
		int tempz;

		if (xRot) {
			tempx = Mathf.RoundToInt (Random.Range (-3, 3)) * 90;
		} else
			tempx = 0;

		if (yRot) {
			tempy = Mathf.RoundToInt (Random.Range (-3, 3)) * 90;
		} else
			tempy = 0;

		if (zRot) {
			tempz = Mathf.RoundToInt (Random.Range (-3, 3)) * 90;
		} else
			tempz = 0;
		


		for (int i = 0; i < targets.Length; i++) 
		{
			Vector3 randDirection = new Vector3 (tempx, tempy, tempz);
			rotationVectors [i] = Quaternion.Euler (randDirection);

		}


	}

	void Start()
	{
		setRotationVector ();
	}

	public void checkRotations()
	{
		int count = 0;
		for (int i = 0; i < targets.Length; i++) 
		{
			if (Mathf.RoundToInt(Quaternion.Angle(targets [i].transform.rotation, rotationVectors [i])) == 0)
				count++;
			} 


		if (count == targets.Length)
			setRotationVector ();

	}

	// Update is called once per frame
	
	void FixedUpdate () {
		for (int i = 0; i < targets.Length; i++) 
		{
			targets [i].transform.rotation = Quaternion.Lerp (targets[i].transform.rotation, rotationVectors [i], FindObjectOfType<itemBank>().getMoveSpeed()*speed*Time.deltaTime);
		}
		checkRotations ();

	}
}
