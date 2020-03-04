using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class powerUpItem : MonoBehaviour {

	public GameObject itemParams;

	private int itemCost;

	private string itemType = "PowerUp";
	private string itemName;
	private string itemDesc;
	private Vector3 itemColor;
	private Vector3 creditTypeColor;

	public void setupParams()
	{
		itemColor = new Vector3 (transform.GetComponent<Image> ().color.r, transform.GetComponent<Image> ().color.g, transform.GetComponent<Image> ().color.b);
		foreach (Transform child in itemParams.transform) {
			if (child.tag == "itemName") {
				itemName = child.GetComponent<Text> ().text;
			} else if (child.tag == "itemDesc") {
				itemDesc = child.GetComponent<Text> ().text;
			}
			else if (child.tag == "itemCost") {
				creditTypeColor = new Vector3(child.GetComponent<Image> ().color.r, child.GetComponent<Image> ().color.g, child.GetComponent<Image> ().color.b);
				int.TryParse(child.GetComponentInChildren<Text> ().text, out itemCost);
			}
		}

	}

	void Awake()
	{
		setupParams ();

	}

	public string getName()
	{
		return itemName;
	}

	public string getType()
	{
		return itemType;
	}

	public string getDescription()
	{
		return itemDesc;
	}

	public int getCost()
	{
		return itemCost;
	}

	public void onItemClick()
	{
		FindObjectOfType<purchaseWindowManager> ().setPurchaseParam (itemName, itemDesc, itemCost, creditTypeColor, itemType, itemColor);
		FindObjectOfType<storeManager> ().init_PurchaseScreen();
		FindObjectOfType<AudioScript> ().playMenuSFX ();


	}
}
