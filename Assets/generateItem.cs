using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class generateItem : MonoBehaviour {

	public GameObject itemParams;

	public void setItemParams(string name, string desc)
	{
		foreach (Transform child in itemParams.transform) {
			if (child.tag == "itemName") {
				child.GetComponent<Text> ().text = name;
			}

			if (child.tag == "itemDesc") {
				child.GetComponent<Text> ().text = desc;
			}
		}

		itemParams.transform.parent.GetComponent<Image> ().color = FindObjectOfType<itemBank>().getItemColor(name);
	}

}
