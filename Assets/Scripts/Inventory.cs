using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Inventory : MonoBehaviour {

	private List<powerUpItem> inventory;	

	public GameObject scrollView;
	private Vector3 og_position;
	public bool inventoryOpen = false;
	private List<System.Action> toAnimate = new List<System.Action> ();

	void Awake()
	{
		og_position = scrollView.transform.localPosition;

	}

	public GameObject parseInGameItemSynthesis(int i)
	{
			if (FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemCostType () == "Credits") {
				
				return Instantiate (Resources.Load ("Inventory/regularInGameItem")) as GameObject;
			} else if (FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemCostType () == "silverCredits") {
				return Instantiate (Resources.Load ("Inventory/silverInGameItem")) as GameObject;
			} else
				return Instantiate (Resources.Load ("Inventory/regularInGameItem")) as GameObject;
		
	}

	public GameObject parseItemSynthesis(int i)
	{
			if (FindObjectOfType<itemBank> ().getItemList () [i].getItemCostType () == "Credits") {
				return Instantiate (Resources.Load ("Inventory/regularItem")) as GameObject;
			} else if (FindObjectOfType<itemBank> ().getItemList () [i].getItemCostType () == "silverCredits") 
			{
				return Instantiate (Resources.Load ("Inventory/silverItem")) as GameObject;
			} 
		return Instantiate (Resources.Load ("Inventory/regularItem")) as GameObject;
	}

	private void showInventory()
	{
		scrollView.transform.localPosition = Vector3.MoveTowards (scrollView.transform.localPosition, new Vector3 (0, scrollView.transform.localPosition.y, 0), 1500 * Time.deltaTime);
		if (scrollView.transform.localPosition.x == 0) {
			toAnimate.Remove (showInventory);
			inventoryOpen = true;

		}
	}

	private void closeInventory()
	{
		scrollView.transform.localPosition = Vector3.MoveTowards (scrollView.transform.localPosition, new Vector3(og_position.x, scrollView.transform.localPosition.y, 0) , 1500 * Time.deltaTime);
		if (scrollView.transform.localPosition.x == og_position.x) {
			toAnimate.Remove (closeInventory);
			inventoryOpen = false;

		}
		
	}

	public void init_closeInventory()
	{
		if (toAnimate.Contains (showInventory))
			toAnimate.Remove (showInventory);
		toAnimate.Add (closeInventory);
	}

	public void init_showInventory()
	{
		toAnimate.Add (showInventory);
	}
		
	void Start()
	{
		for (int i = 0; i < FindObjectOfType<itemBank> ().getItemList().Count; i++) 
		{

			if (FindObjectOfType<playerObj>().getItemCount(FindObjectOfType<itemBank> ().getItemList()[i].getItemName()) > 0)
				{ 
				
				GameObject item = parseItemSynthesis (i);

				item.GetComponentInChildren<invParamSet> ().refresh (FindObjectOfType<itemBank> ().getItemList () [i].getItemName (), FindObjectOfType<itemBank> ().getItemList () [i].getItemDescription (), FindObjectOfType<playerObj>().getItemCount((FindObjectOfType<itemBank> ().getItemList()[i].getItemName())).ToString());
				item.name = FindObjectOfType<itemBank> ().getItemList () [i].getItemName ();
		
				item.transform.SetParent (GameObject.FindGameObjectWithTag ("powerUpContent").transform, false);

				//item.GetComponent<Button> ().onClick.AddListener (equipItem);

				}
		}


		for (int i = 0; i < FindObjectOfType<itemBank> ().getInGameItemList().Count; i++) 
		{

			if (FindObjectOfType<playerObj>().getItemCount(FindObjectOfType<itemBank> ().getInGameItemList()[i].getItemName()) > 0)
			{ 

				GameObject item = parseInGameItemSynthesis (i);

				item.GetComponentInChildren<invParamSet> ().refresh (FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemName (), FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemDescription (), FindObjectOfType<playerObj>().getItemCount((FindObjectOfType<itemBank> ().getInGameItemList()[i].getItemName())).ToString());
				item.name = FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemName ();

				item.transform.SetParent (GameObject.FindGameObjectWithTag ("powerUpContent").transform, false);

				//item.GetComponent<Button> ().onClick.AddListener (equipItem);

			}
		}
			
	}

	void Update()
	{
		for (int i = 0; i < toAnimate.Count; i++) {
			toAnimate [i] ();
		}
	}

}
