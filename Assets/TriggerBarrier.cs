using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBarrier : MonoBehaviour {

	private List <System.Action> ToAnimate = new List<System.Action>();
	public string Triggerfunc;

	IEnumerator inflate (GameObject target)
	{
		target.transform.localScale = Vector3.MoveTowards (target.transform.localScale, FindObjectOfType<basic_stagemaster_functions> ().getOgScales () [FindObjectOfType<basic_stagemaster_functions> ().getTargetID (target)], 5 * Time.deltaTime);
		yield return new WaitForSeconds (5);
		//yield return new WaitUntil (() => targetInflated (target));

	}

	//IF LOCAL SCALE IS == ORIGINAL SCALE OF THIS GAME OBJECT
	//
	IEnumerator shrink(GameObject target)
	{
		if (target.transform.localScale.x == 0)
			StartCoroutine ("inflate", target);
		else {
			target.transform.localScale = Vector3.MoveTowards (target.transform.localScale, new Vector3 (0, 0, 0), Time.deltaTime * 5);
			yield return new WaitUntil (() => targetShrunk (target));

		}
	}

	public bool targetInflated (GameObject target)
	{
		if (target.transform.localScale == FindObjectOfType<basic_stagemaster_functions> ().getOgScales () [FindObjectOfType<basic_stagemaster_functions> ().getTargetID (target)])
			return true;
		else
			return false;
	}

	public bool targetShrunk(GameObject target)
	{
		if (target.transform.localScale.x == 0)
			return true;
		else
			return false;
	}

	void OnTriggerEnter(Collider col)
	{
		st (col.gameObject);
		//col.gameObject.transform.position = new Vector3 (col.gameObject.transform.position.x, col.gameObject.transform.position.y+1, col.gameObject.transform.position.z);
	}

	public void st(GameObject target)
	{
		StartCoroutine (Triggerfunc, target);
	}

	//TRIGGER FUNCTION SHRINK

}
