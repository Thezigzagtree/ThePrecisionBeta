using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour {

	private List <System.Action> ToAnimate = new List<System.Action>();
	private Vector3 ogScale;

	void Awake ()
	{
		ogScale = transform.localScale;
	}

	public void FadeAway()
	{
		
		GetComponent<SpriteRenderer> ().color = Vector4.Lerp (GetComponent<SpriteRenderer> ().color, new Color (GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, 0), Time.deltaTime);
		transform.localScale = Vector3.MoveTowards (transform.localScale, new Vector3 (0,0,0), Time.deltaTime/10);
		if (GetComponent<SpriteRenderer> ().color.a == 0)
			ToAnimate.Remove (FadeAway);
	}

	public void relocate(Vector3 pos)
	{
		transform.parent.localPosition = pos;
	}

	public void FadeIn()
	{
		GetComponent<SpriteRenderer> ().color = new Vector4 (GetComponent<SpriteRenderer> ().color.r, GetComponent<SpriteRenderer> ().color.g, GetComponent<SpriteRenderer> ().color.b, 0.25f);
		transform.localScale = ogScale;

	
		if (!ToAnimate.Contains(FadeAway))
			ToAnimate.Add (FadeAway);

	}

	void FixedUpdate()
	{
		for (int i = 0; i < ToAnimate.Count; i++)
			ToAnimate [i] ();
	}

}
