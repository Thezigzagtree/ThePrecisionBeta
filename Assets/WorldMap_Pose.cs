using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMap_Pose : MonoBehaviour {



	public Vector3 rotateTo;

	private float SphereCastRadius = 0.035f;
	public bool changeAudio = false;

	private Vector3 SphereCastOrigin;
	private Vector3 Direction;
	private LayerMask layermask = ~0;	
	public AudioClip swingUpSFX;
	public AudioClip swingDownSFX;
	private float currentTime;
	private Vector3 orthoPos;
	private Quaternion ogRotation;

	private List<System.Action> toAnimate = new List <System.Action>();

	void Awake()
	{
		ogRotation = transform.rotation;
	}

	public void OnClickFunction()
	{
		if (!toAnimate.Contains (scaleUp)) 
		{
			toAnimate.Add (scaleUp);
			if (GameObject.FindGameObjectWithTag ("SoundManager")) {
				if (changeAudio) {
					GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = Random.Range (1f, 2f);
				} else
					GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = 1;
				GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<AudioSource> ().GetComponent<AudioScript> ().worldMapSFXPlayer (swingUpSFX);
			}

		}

		if (toAnimate.Contains (scaleDown)) {
			toAnimate.Remove (scaleDown);

		}
	}


	public void scaleUp()
	{
		currentTime = Time.timeSinceLevelLoad;
		transform.localRotation = Quaternion.Lerp (transform.localRotation, Quaternion.Euler (rotateTo), 2*Time.deltaTime);
		//targets [i].transform.localRotation = Quaternion.Lerp (targets [i].transform.rotation, ogRotations [destinations [i]], FindObjectOfType<itemBank> ().getMoveSpeed () * Time.deltaTime*5);
		if (Vector3.Distance (transform.rotation.eulerAngles, rotateTo) < 5)  {
			toAnimate.Remove (scaleUp);
			toAnimate.Add (scaleDown);
			if (GameObject.FindGameObjectWithTag ("SoundManager"))
				GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<AudioSource> ().GetComponent<AudioScript> ().worldMapSFXPlayer (swingDownSFX);

		} else
			Debug.Log (Vector3.Distance (transform.rotation.eulerAngles, rotateTo));	
	}

	public void scaleDown()
	{
		transform.localRotation = Quaternion.Lerp (transform.localRotation, ogRotation, 2*Time.deltaTime);

		if (Vector3.Distance (transform.rotation.eulerAngles, ogRotation.eulerAngles) < 5) 
		{
			toAnimate.Remove (scaleDown);
			Debug.Log (Time.timeSinceLevelLoad - currentTime);	

		}
		//		Debug.Log (Vector3.Distance (transform.rotation.eulerAngles, ogRotation.eulerAngles));
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
