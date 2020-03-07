using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class populateShapes : MonoBehaviour {

	void Awake()
	{
		for (int i = 0; i < Resources.LoadAll ("AgentShapes").Length; i++) {

			GameObject button = Instantiate (Resources.Load ("Other/ShapeButton")) as GameObject;
			button.transform.SetParent (this.transform);
			button.transform.localScale = new Vector3(2,1,1);
			button.GetComponentInChildren<Text>().text = Resources.LoadAll ("AgentShapes") [i].name;

			//Debug.Log (Resources.LoadAll ("AgentShapes") [i].name);

			if (SaveSystem.GetInt(Resources.LoadAll("AgentShapes") [i].name) < 1)
			{
				Debug.Log(SaveSystem.GetInt(Resources.LoadAll("AgentShapes") [i].name));
				button.GetComponent<GarageShapeChanging> ().setLocked ();
				button.GetComponent<Image> ().color = Color.black;
			}
		}

		sortOrder ();
	}


	public void sortOrder()
	{
		foreach (Transform button in gameObject.transform) {
			if (SaveSystem.GetInt(button.GetComponentInChildren<Text> ().text) > 0)
				button.transform.SetAsFirstSibling ();
			
		}
	}
}

