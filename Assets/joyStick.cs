using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joyStick : MonoBehaviour {

	public GameObject targets;
	private bool touchStart = false;

	private Vector2 pointA;
	private Vector2 pointB;
	public float speed;

	public void rotateObject(Vector3 direction)
	{
			targets.transform.Rotate(direction);
		
	}

	void Awake()
	{
	}

	void Update()
	{
		//Input.GetTouch (0).phase == TouchPhase.Moved)
		if (Input.touches.Length > 0) 
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began)
				//Debug.Log (t.tapCount);
				pointA = Camera.main.ScreenToWorldPoint (new Vector3 (t.position.x, t.position.y, Camera.main.transform.position.z));
		}

		if (Input.touches.Length > 0) 
		{
			Touch t = Input.GetTouch (0);
			touchStart = true;
			pointB = Camera.main.ScreenToWorldPoint (new Vector3 (t.position.x, t.position.y, Camera.main.transform.position.z));
		} else
			touchStart = false;


	}

	void FixedUpdate()
	{
		if (touchStart) {
			Vector3 offset = pointB - pointA;
			Vector3 direction = Vector3.ClampMagnitude (offset, 10.0f);
			targets.transform.Rotate (new Vector3(direction.x, -direction.z, -direction.y));
		}
			
	}
}
