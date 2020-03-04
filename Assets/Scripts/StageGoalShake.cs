using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGoalShake : MonoBehaviour {

	private bool shaking = false;
	private float shaketimer;
	private Vector3 OG_position;
	//	FullHPBar.GetComponent<RectTransform> ().transform.localPosition = new Vector3 (FullHPBar_OGposition.x + Random.Range (-2, 2), FullHPBar_OGposition.y + Random.Range (-2, 2), FullHPBar_OGposition.z + Random.Range (-2, 2));
	// Use this for initialization

	void Start () 
	{
		OG_position = GetComponent<RectTransform> ().transform.localPosition;
	}

	public void Startshaking()
	{
		shaketimer = Time.timeSinceLevelLoad;
		shaking = true;
	}

	public void Resetposition()
	{
		GetComponent<RectTransform> ().transform.localPosition = OG_position;
	}

	// Update is called once per frame
	void Update () 
	{

		if (shaking == true && (Time.timeSinceLevelLoad - shaketimer) < 0.4f) {
			GetComponent<RectTransform> ().transform.localPosition = new Vector3 (OG_position.x + Random.Range (-2, 2), OG_position.y + Random.Range (-2, 2), OG_position.z + Random.Range (-2, 2));
		} else {
		}
			//Resetposition ();
			
	}
}
