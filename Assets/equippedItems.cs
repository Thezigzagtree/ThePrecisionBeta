using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class equippedItems : MonoBehaviour {

	private string equippedItem = "";
	private string inGameItem = "";

	public void setInGameItem(string itemName)
	{
		inGameItem = itemName;
	}

	public string getEquippedItem()
	{
		return equippedItem;
	}

	public void setEquippedItem(string itemName)
	{
		equippedItem = itemName;
		Debug.Log (equippedItem);

	}

	public string getInGameItem()
	{
		return inGameItem;
	}

	public bool isInGameItemEquipped()
	{
		if (inGameItem == "") {
			return false;
		} else
			return true;
	}

	public bool isItemEquipped()
	{
		if (equippedItem == "")
			return false;
		else
			return true;
	}


}
