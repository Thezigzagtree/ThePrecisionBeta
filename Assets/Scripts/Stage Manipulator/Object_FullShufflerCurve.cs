using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_FullShufflerCurve : MonoBehaviour {

		public GameObject StageTargets;
		private GameObject[] targets;

		private List<int> destinations = new List<int>();
		private List<Vector3> ogDestinations = new List<Vector3>();
		private List<int> destInt = new List<int>();
		public int difficulty;

		private float speed;
		private bool targets_reached;

		public void setupOgDestinations()
		{
			for (int i = 0; i < StageTargets.transform.childCount; i++) {
				ogDestinations.Add (StageTargets.transform.GetChild (i).transform.localPosition);
			}
		}

		public void setupDestinations()
		{
			for (int i = 0; i < ogDestinations.Count; i++) {
				int randDest = Random.Range(0, destInt.Count);
				destinations.Add (destInt [randDest]);
				destInt.RemoveAt (randDest);
			}
		}

		public void Interswap()
		{
			destinations.Clear();
			setupWhichDestination ();
			setupDestinations ();
			targets_reached = false;
			speed = Random.Range(3f,5f) + difficulty*0.5f;

		}

		public void setupWhichDestination()
		{
			for (int i = 0; i < StageTargets.transform.childCount; i++) {
				destInt.Add (i);
			}
		}

		void Awake()
		{
			targets = new GameObject[StageTargets.transform.childCount];

			for (int i = 0; i < StageTargets.transform.childCount; i++) {
				targets [i] = StageTargets.transform.GetChild (i).gameObject;	
			}	

			setupOgDestinations ();

			Interswap ();
		}

		public void check_destinations()
		{
			for (int i = 0; i < StageTargets.transform.childCount; i++) 
			{
				if (targets [i].transform.localPosition == ogDestinations [destinations [i]]) {
				} else 
					return;
				if (i == targets.Length - 1)
					targets_reached = true;
			}
		}

		void FixedUpdate()
		{
			for (int i = 0; i < targets.Length; i++) {

			targets [i].transform.localPosition = Vector3.Slerp (targets [i].transform.localPosition, ogDestinations [destinations [i]], FindObjectOfType<itemBank>().getMoveSpeed()*speed*Time.deltaTime);
			}

			check_destinations ();

			if (targets_reached) {
				Interswap ();
			}
		}
	}
