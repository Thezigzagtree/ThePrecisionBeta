using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_ricochet : MonoBehaviour {

	public GameObject[] targets;
	public float speed;
	private Vector3[] directions;

	void Awake()
	{
		directions = new Vector3[targets.Length];
		for (int i = 0; i < targets.Length; i++) 
		{
			directions [i] = new Vector3 (Random.Range (-120f, 120f), Random.Range (-120f, 120f), Random.Range(0, -240f));
		}
	}

	public void checkEdges()
	{
		for (int i = 0; i < targets.Length; i++) {
			targets [i].transform.position = Vector3.MoveTowards (targets [i].transform.position, directions [i], FindObjectOfType<itemBank>().getMoveSpeed()*speed*Time.deltaTime);
			if (targets [i].transform.position.x <= -3.5f || targets [i].transform.position.x >= 3.5f) {
				directions [i] = Vector3.Reflect (directions [i], new Vector3(1,0,0));
			}


			else if (targets [i].transform.position.y >= 5 || targets [i].transform.position.y <= -4.5f) 
			{
				directions [i] = Vector3.Reflect (directions [i], new Vector3(0,1,0));

			}

			else if (targets [i].transform.position.z >= 5 || targets [i].transform.position.z <= -2f) 
			{
				directions [i] = Vector3.Reflect (directions [i], new Vector3 (0, 0, 1));
			}
		}
	}
		

	void FixedUpdate()
	{
		
		checkEdges ();

	}
}
