using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Teleporter : MonoBehaviour {

	public GameObject target;

	private Vector3 Og_position;
	private Vector3 Og_scale;
	private Vector3 Shrinkmethod = new Vector3 (0,1,0);

	public Vector3[] destinations;
	private Vector3[] positions;
	//public Vector3[] Teleportdestinations;
	public float ShrinkTime;
	public float ExpandTime;
	public float SmoothTime;


	private Vector3 DampVelocity;

	private System.Action State;


	private int pos = 0;
	private float expandtiming;

	void Awake()
	{
		Og_scale = target.transform.localScale;
		positions = new Vector3[destinations.Length + 1];
		positions [0] = target.transform.position;

	}

	public int Shiftposition()
	{
		if (pos < positions.Length - 1)
			return pos += 1;
		else
			return pos = 0;
	}

	public void StartExpanding()
	{
		target.transform.localScale = Vector3.SmoothDamp (target.transform.localScale, Og_scale, ref DampVelocity, ExpandTime/FindObjectOfType<itemBank>().getMoveSpeed());
		if (Time.timeSinceLevelLoad - expandtiming >=  4*SmoothTime) 
		{
			pos = Shiftposition ();
			State = StartMoving;
		}
	//	yield return new WaitUntil (() => expandtiming - Time.timeSinceLevelLoad >= 2*SmoothTime);
	//	ToAnimate.Remove (Expand);
		//	ToAnimate.Add (Shrink);
	}

	public void StartMoving()
	{
		target.transform.position = Vector3.SmoothDamp (target.transform.position, positions [pos], ref DampVelocity, SmoothTime/FindObjectOfType<itemBank>().getMoveSpeed());
		if (target.transform.position == positions [pos]) 
		{
			State = StartShrinking;

		}
	}

	public void TeleportTo(Vector3 loc)
	{
		target.transform.position = loc;
		expandtiming = Time.timeSinceLevelLoad;
		State = StartExpanding;
	}

	public void StartShrinking()
	{
		target.transform.localScale = Vector3.SmoothDamp (target.transform.localScale, Shrinkmethod, ref DampVelocity, ShrinkTime/FindObjectOfType<itemBank>().getMoveSpeed());
		if (target.transform.localScale == Shrinkmethod) 
		{
			pos = Shiftposition ();
			TeleportTo (positions [pos]);
			State = StartExpanding;
		}

	}


	void Start()
	{
		for (int i = 1; i < positions.Length; i++) {
			positions [i] = destinations [i-1];
		}

		State = StartShrinking;
	}
		
	void FixedUpdate()
	{

		State ();
	}
}
