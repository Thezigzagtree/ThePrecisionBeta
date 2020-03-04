using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMap_ParticleActivator : MonoBehaviour {
	private float SphereCastRadius = 0.035f;
	public bool changeAudio = false;
	private Vector3 SphereCastOrigin;
	private Vector3 Direction;
	private LayerMask layermask = ~0;	

	public AudioClip SoundEffect;
	private Vector3 orthoPos;
	public void OnClickFunction()
	{
		
		if (!GetComponent<ParticleSystem> ().isPlaying) {
			GetComponent<ParticleSystem> ().Clear();
			GetComponent<ParticleSystem> ().Play();
			if (GameObject.FindGameObjectWithTag ("SoundManager")) {
				if (changeAudio) {
					GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = Random.Range (0.5f, 2f);
				} else
					GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = 1;
				GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<AudioSource> ().GetComponent<AudioScript> ().worldMapSFXPlayer (SoundEffect);
			}
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
