using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadAgentShape : MonoBehaviour {


	private GameObject shape;
	void Awake()
	{
		LoadAgent ();
	}

	public void LoadAgent()
//FindObjectOfType<playerObj>().currentAgentShape
	{
		shape = Instantiate (Resources.Load ("AgentShapes/" + SaveSystem.GetString("currentAgentShape"))) as GameObject;
		//	shape = Instantiate (Resources.Load("AgentShapes/Battery")) as GameObject;
			shape.transform.SetParent (this.transform);
			shape.transform.localPosition = new Vector3 (0, 4, 0);
			shape.GetComponent<Light> ().color = new Color (PlayerPrefs.GetFloat ("redLight"), PlayerPrefs.GetFloat ("greenLight"), PlayerPrefs.GetFloat ("blueLight"));
			

	}

	public void displayAgent(string shapeName )
	{
		Destroy (GameObject.FindGameObjectWithTag ("Player").transform.GetChild (0).gameObject);
		shape = Instantiate (Resources.Load ("AgentShapes/" + shapeName)) as GameObject;
			//GameObject shape = Instantiate (Resources.Load("AgentShapes/Obelisk")) as GameObject;
		shape.transform.SetParent (this.transform);
		shape.transform.localPosition = new Vector3 (0, 4, 0);
		shape.GetComponent<Light> ().color = Color.black;
		FindObjectOfType<GarageManager> ().lockTextOn ();
	}
}
