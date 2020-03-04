using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldMap_ShapeChanger : MonoBehaviour {
	public bool colorChange;
		public AudioClip Soundeffect1;
		public AudioClip Soundeffect2;
	public bool changeAudio = false;
		private float SphereCastRadius = 0.035f;
		private Vector3 SphereCastOrigin;
		private Vector3 Direction;
		private LayerMask layermask = ~0;	
		public GameObject[] mutatorShapes;
	public Vector3[] shapeOrientations;
	private Vector3 ogScale;
		private Vector3 orthoPos;
		private bool shapeChanged = false;
		private bool shapeChanging = false;

	void Awake()
	{	
		ogScale = transform.localScale;
		if(shapeOrientations.Length == 0)
			shapeOrientations = new Vector3[mutatorShapes.Length];
		
	}
	public void changeShape()
	{
		int x = Random.Range (0, mutatorShapes.Length);
		GetComponent<MeshFilter> ().mesh = mutatorShapes [x].GetComponent<MeshFilter>().sharedMesh;
		transform.rotation = Quaternion.Euler (shapeOrientations [x]);
		shapeChanging = false;
		shapeChanged = true;
		if(GameObject.FindGameObjectWithTag("SoundManager"))
			GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>().GetComponent<AudioScript> ().worldMapSFXPlayer (Soundeffect2);
		

	}

		public void OnClickFunction()
		{
		shapeChanging = true;
		if (GameObject.FindGameObjectWithTag ("SoundManager")) {
			if (changeAudio) {
				GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = Random.Range (1f, 2f);
			} else
				GameObject.FindGameObjectWithTag ("mapSFXManager").GetComponent<AudioSource> ().pitch = 1;
			GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<AudioSource> ().GetComponent<AudioScript> ().worldMapSFXPlayer (Soundeffect1);
		}

		}


		void FixedUpdate()
	{
		if (shapeChanging == true) 
		{
			transform.localScale = Vector3.MoveTowards (transform.localScale, new Vector3 (0, 0, 0), 10 * Time.deltaTime);	
			if (transform.localScale == new Vector3 (0, 0, 0)) 
			{

				changeShape ();
				if(colorChange)
					GetComponent<MeshRenderer> ().material.SetColor ("_ASEOutlineColor", new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f),Random.Range(0.5f, 1f)));
				
			} 

			
		} 
		else if (shapeChanged == true) 
		{
			transform.localScale = Vector3.Lerp (transform.localScale, ogScale, 5 * Time.deltaTime);
			if (transform.localScale == ogScale) {
				shapeChanged = false;

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

