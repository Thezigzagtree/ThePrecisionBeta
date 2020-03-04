using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_PushPull : MonoBehaviour {

	public GameObject[] targets;
	public float speed;
	public GameObject center;
	public float minDist;
	public float maxDist;
	private bool[] comingBack;



	public Vector3 farPoint(GameObject target)
	{
		Vector3 directionOfTravel = target.transform.position - center.transform.position ;

		Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * maxDist;

		Vector3 targetPosition = target.transform.position + finalDirection;
		return targetPosition;

	}

	public Vector3 closePoint (GameObject target)
	{
		Vector3 directionOfTravel = center.transform.position- target.transform.position;

		Vector3 finalDirection = directionOfTravel + directionOfTravel.normalized * minDist;

		Vector3 targetPosition = target.transform.position + finalDirection;
		return targetPosition;
		
	}

	void Awake()
	{
		comingBack = new bool[targets.Length];
		for (int i = 0; i < comingBack.Length; i++)
			comingBack [i] = true;


	}

	void FixedUpdate()
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			
			if (comingBack [i] == true) 
			{

				targets [i].transform.position = Vector3.MoveTowards (targets [i].transform.position, closePoint (targets [i]), FindObjectOfType<itemBank>().getMoveSpeed()* speed*Time.deltaTime);
				if (Vector3.Distance (targets [i].transform.position, center.transform.position) < minDist)
					comingBack [i] = false;
			} 


			else 
			{
				targets [i].transform.position = Vector3.MoveTowards (targets [i].transform.position, farPoint (targets [i]), FindObjectOfType<itemBank>().getMoveSpeed()* speed*Time.deltaTime);
				if (Vector3.Distance (targets [i].transform.position, center.transform.position) > maxDist)
					comingBack [i] = true;
			}
		}


	}
}
