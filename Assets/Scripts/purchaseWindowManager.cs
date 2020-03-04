using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class purchaseWindowManager : MonoBehaviour {

	public GameObject purchaseParam;
	public GameObject purchaseItem;
	public GameObject purchaseType;

	private Vector3 creditType;
	private string itemName;
	private string itemDesc;
	private int itemCost;
	private string itemType;
	private Vector3 purchaseColor;


	public void updatePurchaseType()
	{
		purchaseType.GetComponentInChildren<Text> ().text = "You are purchasing a " + itemType;
	}

	public void updatePurchase()
	{
		purchaseParam.transform.parent.name = itemName;
		foreach (Transform child in purchaseParam.transform) {
			if (child.tag == "itemName") {
				child.GetComponent<Text> ().text = itemName;
			} 

			else if (child.tag == "itemDesc") {
				child.GetComponent<Text> ().text = itemDesc;
			}
				
			else if (child.tag == "itemCost") {
				child.GetComponent<Image> ().color = new Color (creditType.x, creditType.y, creditType.z);
				child.GetComponentInChildren<Text> ().text = itemCost.ToString();
			}
		}

		purchaseItem.GetComponent<Image> ().color = new Color (purchaseColor.x, purchaseColor.y, purchaseColor.z);
		updatePurchaseType ();
	}


	public void setPurchaseParam(string name, string desc, int cost, Vector3 ctype, string type, Vector3 col)
	{
		purchaseColor = col;
		creditType = ctype;
		itemName = name;
		itemDesc = desc;
		itemCost = cost;
		itemType = type;

		updatePurchase ();
	}
}
