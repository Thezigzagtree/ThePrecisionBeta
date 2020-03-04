using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class invParamSet : MonoBehaviour {

	public Text itemName;
	public Text itemDesc;
	public Text itemQuantity;

	public void equipItem(GameObject itemParams)
	{

		if (GameObject.FindObjectOfType<AudioScript> () != null)
			FindObjectOfType<AudioScript> ().playMenuSFX ();
		GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().setEquippedItem (itemParams.GetComponent<invParamSet>().itemName.text);
		GameObject.FindGameObjectWithTag ("powerUpContent").GetComponent<Inventory> ().init_closeInventory ();
		FindObjectOfType<StageDetailToggle> ().setAppliedItemText (itemParams.GetComponent<invParamSet>().itemName.text);
		FindObjectOfType<StageDetailToggle> ().revealRemoveItemButton ();

	}


	public void refresh(string n, string d, string c)
	{
		itemName.text = n;
		itemDesc.text = d;
		itemQuantity.text = "x"+c;
		
	}


}
	
