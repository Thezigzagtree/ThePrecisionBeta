using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystalLine : MonoBehaviour {
	private LineRenderer line;
	//public GameObject lockSymbol;
	void Awake()
	{
		line = gameObject.AddComponent<LineRenderer> ();
		line.useWorldSpace = true;
		line.sharedMaterial = FindObjectOfType<sceneryManager> ().stageLockMat;
		line.startColor = new Color (0.8f, 0, 0, 0.05f);
		line.endColor = new Color (0.8f, 0.8f, 0, 1);

	}

	void Update()
	{
		
			for (int i = 0; i < transform.parent.transform.parent.GetComponent<stageLock> ().bridgeTo.Count * 2; i++) {
				line.startWidth = 0.55f;
				line.endWidth = 0.15f;
				line.positionCount = transform.parent.transform.parent.GetComponent<stageLock> ().bridgeTo.Count * 2;

				if (i % 2 == 0) {
					line.SetPosition (i, transform.position);

				} else {
					line.SetPosition (i, transform.parent.transform.parent.GetComponent<stageLock> ().bridgeTo [i / 2].transform.position);


				}

			}


		} 
	}

