using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class WorldMap_Scaler : MonoBehaviour {


	public bool colorChange;
	public Vector3 scaleTo;


	private float SphereCastRadius = 0.035f;
	public bool changeAudio = false;
	private Vector3 SphereCastOrigin;
	private Vector3 Direction;
	private LayerMask layermask = ~0;	
	public AudioClip scaleUpSFX;
	public AudioClip scaleDownSFX;

	private float currentTime;

	private Vector3 orthoPos;
	private Vector3 ogScale;

	private List<System.Action> toAnimate = new List <System.Action>();

	void Awake()
	{
		ogScale = transform.localScale;
	}

	public void OnClickFunction()
	{
		if (!toAnimate.Contains (scaleUp)) {
			currentTime = Time.timeSinceLevelLoad;
			toAnimate.Add (scaleUp);
			if (GameObject.FindGameObjectWithTag ("SoundManager")) {
				if (changeAudio) {
					GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = Random.Range (1f, 2f);
				} else
					GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = 1;
				
				GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<AudioSource> ().GetComponent<AudioScript> ().worldMapSFXPlayer (scaleUpSFX);
			}
		}

		if (toAnimate.Contains (scaleDown)) {
			toAnimate.Remove (scaleDown);

			Debug.Log (Time.timeSinceLevelLoad - currentTime);

		}
	}


	public void scaleUp()
	{
		transform.localScale = Vector3.Lerp (transform.localScale, scaleTo, Time.deltaTime * 8);
		if (Vector3.Distance(transform.localScale, scaleTo) <= 0.1f) {
			Debug.Log (Time.timeSinceLevelLoad - currentTime);
			toAnimate.Remove (scaleUp);
			toAnimate.Add (scaleDown);
			if(colorChange)
				GetComponent<MeshRenderer> ().material.SetColor ("_ASEOutlineColor", new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f),Random.Range(0.5f, 1f)));

			if(GameObject.FindGameObjectWithTag("SoundManager"))
				GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>().GetComponent<AudioScript> ().worldMapSFXPlayer (scaleDownSFX);


		}
	}

	public void scaleDown()
	{
		transform.localScale = Vector3.Lerp (transform.localScale, ogScale, Time.deltaTime * 12);

		if (transform.localScale == ogScale) {
			toAnimate.Remove (scaleDown);

		}
		
	}
		
	void Update()
	{
		if (Input.GetMouseButtonDown(0)) 
		{
			orthoPos = Input.mousePosition;
			orthoPos.z = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ().nearClipPlane;
			orthoPos = Camera.main.ScreenToWorldPoint (orthoPos);
			//orthoPos.Normalize ();


			//SphereCastOrigin = GameObject.FindGameObjectWithTag ("MainCamera").transform.position;


			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if (EventSystem.current.IsPointerOverGameObject ())
				return;

			if (Physics.SphereCast (orthoPos, SphereCastRadius, ray.direction, out hit, 100f, layermask))
			{
				if (hit.collider.transform == gameObject.transform) {

					OnClickFunction ();
				} 
			}
		}

		for (int i = 0; i < toAnimate.Count; i++) {
			toAnimate [i] ();
		}


	}
}
