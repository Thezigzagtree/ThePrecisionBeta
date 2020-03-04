using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemParamSet : MonoBehaviour {

	public Text itemName;
	public Text itemDesc;
	public Text itemCost;

	public void refresh(string n, string d, string c)
	{
		itemName.text = n;
		itemDesc.text = d;
		itemCost.text = c;
		
	}
}
