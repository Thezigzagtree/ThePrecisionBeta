using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMap_Slider : MonoBehaviour {

	public Vector3 rotateIncrement;

	private float SphereCastRadius = 0.035f;
	public bool changeAudio = false;
	private Vector3 rotateTo;
	private Vector3 SphereCastOrigin;
	private Vector3 Direction;
	private LayerMask layermask = ~0;	
	public AudioClip swingUpSFX;

	private Vector3 orthoPos;
	private Quaternion ogRotation;

	private List<System.Action> toAnimate = new List <System.Action>();

	void Awake()
	{
		ogRotation = transform.rotation;
	}

	public void OnClickFunction()
	{
		if (!toAnimate.Contains (scaleUp)) {
			rotateTo = transform.rotation.eulerAngles + rotateIncrement;
			if (rotateTo.x >= 360)
				rotateTo = new Vector3 (rotateTo.x - 360, rotateTo.y, rotateTo.z);
			if (rotateTo.y >= 360)
				rotateTo = new Vector3 (rotateTo.x, rotateTo.y - 360, rotateTo.z);
			if (rotateTo.z >= 360)
				rotateTo = new Vector3 (rotateTo.x, rotateTo.y, rotateTo.z - 360);
			toAnimate.Add (scaleUp);
			if (GameObject.FindGameObjectWithTag ("SoundManager")) {
				if (changeAudio) {
					GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = Random.Range (1f, 2f);
				} else
					GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = 1;
				GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<AudioSource> ().GetComponent<AudioScript> ().worldMapSFXPlayer (swingUpSFX);
			}

		}
	
	}


	public void scaleUp()
	{
		
		transform.localRotation = Quaternion.RotateTowards (transform.localRotation, Quaternion.Euler (rotateTo), 7.5f);
		//targets [i].transform.localRotation = Quaternion.Lerp (targets [i].transform.rotation, ogRotations [destinations [i]], FindObjectOfType<itemBank> ().getMoveSpeed () * Time.deltaTime*5);
		if (Vector3.Distance (transform.rotation.eulerAngles, rotateTo) < 1)  
		{
			toAnimate.Remove (scaleUp);


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
