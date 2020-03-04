using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_FullSwapper : MonoBehaviour {


	private List<Vector3> DampVelocities = new List<Vector3>();

	public GameObject StageTargets;
	private List<GameObject> targets = new List<GameObject>();

	private List<List<int>> swapPair = new List<List<int>>();
	private List<int> destinations = new List<int>();
	private List<Vector3> ogDestinations = new List<Vector3>();
	private List<int> swapPairSizes = new List<int>();
	private List<int> destInt = new List<int>();
	private List<float> swapSpeeds = new List<float> ();
	public int difficulty;
		
	private bool targets_reached;


	public void setupOgDestinations()
	{
		for (int i = 0; i < StageTargets.transform.childCount; i++) {
			ogDestinations.Add (StageTargets.transform.GetChild (i).transform.localPosition);
			DampVelocities.Add(StageTargets.transform.GetChild(i).transform.localPosition);
		}
	}

	public void setupSwapPairsSizes()
	{
		int count = 0;
		while (count != targets.Count) 
		{

			int randPair = Random.Range (2, 5);
			if (randPair + count <= targets.Count) {
				swapPairSizes.Add (randPair);
				count += randPair;
			} else {
				swapPairSizes.Add (targets.Count - count);
				count = targets.Count;
			}
		}
	}



	public void setupDestinations()
	{
		for (int i = 0; i < swapPairSizes.Count; i++) 
		{
			List<int> currentPair = new List<int> ();

			for (int j = 0; j < swapPairSizes [i]; j++) 
			{
				int randDest = Random.Range (0, destInt.Count);
				//destinations.Add (destInt [randDest]);
				currentPair.Add(destInt [randDest]);
				destInt.RemoveAt (randDest);
			}

			swapPair.Add (currentPair);
		}
			

	}
		
	public void clearLists()
	{
		destinations.Clear();
		swapPairSizes.Clear ();
		swapPair.Clear ();
		swapSpeeds.Clear ();
	}

	public void Interswap()
	{
		
		clearLists ();


		setupWhichDestination ();

		setupSwapPairsSizes ();

		setupDestinations ();
		targets_reached = false;
		setupSwapSpeeds ();
	}


	public void setupSwapSpeeds()
	{
		for (int i = 0; i < swapPair.Count; i++) {
			swapSpeeds.Add (Random.Range (2f, 4f)+(difficulty*0.5f));
		}

	}



	public void setupWhichDestination()
	{
		for (int i = 0; i < StageTargets.transform.childCount; i++) {
			destInt.Add (i);
		}
	}
	void Awake()
	{
		
		for (int i = 0; i < StageTargets.transform.childCount; i++) 
		{
			targets.Add(StageTargets.transform.GetChild (i).gameObject);	
		}	

		setupOgDestinations ();


	}

	void Start()
	{
		Interswap ();
	}


	public void swap_targets()
	{
		/*
		 * for (int i = 0; i < targets.Length; i++) 
		{
			targets [i].transform.locallocalPosition = Vector3.Lerp (targets [i].transform.locallocalPosition, destinations [i], swapspeed*3 * Time.deltaTime);
			if (targets_reached) 
			{
				setup_destinations ();
				targets_reached = false;
			}
		}	
		 */
		for (int i = 0; i < swapPair.Count; i++) 
		{
			for (int j = 0; j < swapPair[i].Count-1; j++) 
			{
				StageTargets.transform.GetChild (swapPair [i] [j]).transform.localPosition = Vector3.MoveTowards (StageTargets.transform.GetChild (swapPair [i] [j]).transform.localPosition, ogDestinations [swapPair [i] [j + 1]], Time.deltaTime * swapSpeeds [i] *FindObjectOfType<itemBank>().getMoveSpeed());
					
				}

			StageTargets.transform.GetChild (swapPair [i] [swapPair [i].Count - 1]).transform.localPosition = Vector3.MoveTowards (StageTargets.transform.GetChild (swapPair [i] [swapPair [i].Count - 1]).transform.localPosition, ogDestinations [swapPair [i] [0]], Time.deltaTime * swapSpeeds [i]*FindObjectOfType<itemBank>().getMoveSpeed());
				//Vector3.Lerp (StageTargets.transform.GetChild (swapPair [i] [swapPair[i].Count-1]).transform.localPosition, ogDestinations[swapPair [i] [0]], swapSpeeds[i]*Time.deltaTime);
		
		}
	}

	public void check_destinations()
	{
		int count = 0;
		for (int i = 0; i < swapPair.Count; i++) 
		{
			for (int j = 0; j < swapPair [i].Count - 1; j++) 
			{
				if (StageTargets.transform.GetChild (swapPair [i] [j]).transform.localPosition == ogDestinations [swapPair [i] [j + 1]]) {
					count += 1;
				} else
					return;

			}
			if (StageTargets.transform.GetChild (swapPair [i] [swapPair [i].Count - 1]).transform.localPosition == ogDestinations [swapPair [i] [0]]) {
				count += 1;
			} else
				return;

		}

		if (count == targets.Count)
			targets_reached = true;	
		
	}



	void FixedUpdate()
	{
		swap_targets ();
		check_destinations ();

		if (targets_reached) {
			Interswap ();
			targets_reached = false;
		}
	}
}
