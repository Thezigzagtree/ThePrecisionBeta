using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class worldMap_Rotator : MonoBehaviour {

	public bool colorChange;
	private float time = 0;
	public bool changeAudio = false;
	public AudioClip Soundeffect;
	private float SphereCastRadius = 0.035f;
	private Vector3 SphereCastOrigin;
	private Vector3 Direction;
	private LayerMask layermask = ~0;	

	private Vector3 orthoPos;


	public void OnClickFunction()
	{
		time = Time.timeSinceLevelLoad + 1;
		if(colorChange)
			GetComponent<MeshRenderer> ().material.SetColor ("_ASEOutlineColor", new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f),Random.Range(0.5f, 1f)));
		
		if (GameObject.FindGameObjectWithTag ("SoundManager")) {
			if (changeAudio) {
				GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = Random.Range (1f, 2f);
			} else
				GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = 1;
			GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<AudioSource> ().GetComponent<AudioScript> ().worldMapSFXPlayer (Soundeffect);

		}
	
	}


	void FixedUpdate()
	{
		if (Time.timeSinceLevelLoad > time)
			transform.Rotate (new Vector3 (transform.rotation.x, 10, transform.rotation.z) * (Time.deltaTime)); //REGULAR CONDITION
		else {
			transform.Rotate (new Vector3 (0, 10, 0) * (15f * Time.deltaTime)); //CLICKED CONDITION
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

	}
}
