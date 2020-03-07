using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JSNodeMap;
using UnityEngine.EventSystems;

public class StageDetailToggle : MonoBehaviour {
		
		public Agent agent;
		private Vector3 og_pos;
		private List <System.Action> ToAnimate = new List<System.Action>();
		private GameObject Rankstar;
		private bool detailRemoving = false;
		public Text appliedItemText;
		public Button removeItemButton;
		public Button Startbutton;
		
	void Awake () {
		
		removeItemButton.GetComponent<Image>().enabled = false;
		Rankstar = GameObject.FindGameObjectWithTag ("Rank");
		agent.OnMoveStart += hideStageDetail;
		agent.OnMoveEnd += showStageDetail;
		agent.OnMoveEnd += updateText;
		}

	public void initHideStageDetail()
	{
		removeRank ();
		ToAnimate.Add (removeUI);

		
	}
	public void removeEquippedItem()
	{
		GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().setEquippedItem ("");
		setAppliedItemText ("");
		removeItemButton.GetComponent<Image>().enabled = false;
	}

	public void setAppliedItemText(string s)
	{
		if (s == "") {
			appliedItemText.text = "- - No Item Applied - -";
		} else
		{
			appliedItemText.text = ("- " + s + " Applied -");
		}
	}

	public void revealRemoveItemButton()
	{
		removeItemButton.GetComponent<Image>().enabled = true;
	}


	public void removeRank()
	{
		Rankstar.transform.GetChild (0).GetComponent<Image> ().color = new Color (0.2f, 0.2f, 0.2f, 1);
		Rankstar.transform.GetChild (1).GetComponent<Image> ().color = new Color (0.2f, 0.2f, 0.2f, 1);
		Rankstar.transform.GetChild (2).GetComponent<Image> ().color = new Color (0.2f, 0.2f, 0.2f, 1);
	}

	public void hideRank()
	{
		Rankstar.transform.GetChild (0).GetComponent<Image> ().color = new Color (0.2f, 0.2f, 0.2f, 0);
		Rankstar.transform.GetChild (1).GetComponent<Image> ().color = new Color (0.2f, 0.2f, 0.2f, 0);
		Rankstar.transform.GetChild (2).GetComponent<Image> ().color = new Color (0.2f, 0.2f, 0.2f, 0);

	}

	public void showRank()
	{
		
		if (FindObjectOfType<playerObj>().playerHasStage(agent.currentNode.name) && FindObjectOfType<playerObj>().getStageRank(agent.currentNode.name) != 0) {

			for (int i = 0; i < FindObjectOfType<playerObj>().getStageRank(agent.currentNode.name); i++) {
				Rankstar.transform.GetChild (i).GetComponent<Image> ().color = new Color (199, 148, 14, 1);
			}
		} else if (FindObjectOfType<playerObj>().getStageRank(agent.currentNode.name) == 0) 
		{
			hideRank ();
		}
		else
	{
			removeRank ();
		}

	
	}

	public string removeUnderscore(string str)
	{
		if (str.Substring (0, 5).ToUpper() == "Stage".ToUpper())
			return "Stage " + str.Substring (str.Length - 4);
		else
			return str;
	}
		
	public void displayText()
	{
			GameObject.FindGameObjectWithTag ("StageText").GetComponent<Text> ().text = removeUnderscore (agent.currentNode.name);
			//GameObject.FindGameObjectWithTag ("StageText").GetComponent<TextMeshProUGUI> ().SetText (GameObject.FindGameObjectWithTag ("StageStats").GetComponent<stageStats> ().StageAsNick (agent.currentNode.name));
		
		
	}
	public void Start()
	{
		og_pos = GetComponent<RectTransform>().localPosition;
	}
	public void returnUI()
	{
		GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GetComponent<RectTransform>().localPosition, new Vector3(og_pos.x-800,og_pos.y,og_pos.z), 100f);
		if (GetComponent<RectTransform> ().localPosition.x == og_pos.x-800) 
		{
			detailRemoving = false;
			ToAnimate.Remove (returnUI);
		}

		//ShowCorrectHit(new Vector3 (540, 960, 0));
	}

	void updateText (Node targetNode) {
		//if (GameObject.FindGameObjectWithTag ("StageStats"))
			//GameObject.FindGameObjectWithTag ("StageText").GetComponent<Text> ().text = (GameObject.FindGameObjectWithTag ("StageStats").GetComponent<stageStats> ().StageAsNick (targetNode.name));
		//GameObject.FindGameObjectWithTag ("StageText").GetComponent<TextMeshProUGUI> ().SetText (GameObject.FindGameObjectWithTag ("StageStats").GetComponent<stageStats> ().StageAsNick (targetNode.name));
			//GameObject.FindGameObjectWithTag ("StageText").GetComponent<TextMeshProUGUI> ().SetText (removeUnderscore (agent.currentNode.name));
			GameObject.FindGameObjectWithTag ("StageText").GetComponent<Text> ().text = (removeUnderscore (agent.currentNode.name));
		
		//else
		//	GameObject.FindGameObjectWithTag ("StageText").GetComponent<Text> ().text = (targetNode.name);
//		GameObject.FindGameObjectWithTag ("StageText").GetComponent<TextMeshProUGUI> ().SetText (targetNode.name);
	}


	void hideUI (Node targetNode) {
		ToAnimate.Add (removeUI);
	}

	// Use this for initialization
	void hideStageDetail (Node targetNode) {
		removeRank ();
		
		ToAnimate.Add (removeUI);
	}
	void showStageDetail (Node targetNode) {

		if(targetNode.GetComponent<starLock>())
		{
			Startbutton.interactable = false;
			Startbutton.GetComponentInChildren<Text>().text = "Star Locked";
		}
			
		else
		{
			Startbutton.interactable = true;
			Startbutton.GetComponentInChildren<Text>().text = "Start";
		
		}
			
		//Debug.Log (agent.agentType);
		showRank();
		if (FindObjectOfType<playerObj>().playerHasStage(targetNode.name) || SaveSystem.GetInt("credits")-300 < 0 || targetNode.GetComponent<starLock>()) {
			GameObject.FindGameObjectWithTag ("skipButton").GetComponent<Button> ().interactable = false;
			GameObject.FindGameObjectWithTag ("skipButton").GetComponentInChildren<Text> ().enabled = false;
		} else {
			GameObject.FindGameObjectWithTag ("skipButton").GetComponent<Button> ().interactable = true;
			GameObject.FindGameObjectWithTag ("skipButton").GetComponentInChildren<Text> ().enabled = true;

		}
		ToAnimate.Remove (removeUI);
		ToAnimate.Add (returnUI);

	}

	public void removeUI()
	{
		GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(GetComponent<RectTransform>().localPosition, new Vector3(og_pos.x, GetComponent<RectTransform>().localPosition.y, 0), 100f);
		if (GetComponent<RectTransform> ().localPosition.x == og_pos.x) {
			ToAnimate.Remove (removeUI);
		}

	}

	void Update()
	{	
		for (int i = 0; i < ToAnimate.Count; i++) {
			ToAnimate [i] ();
		}

		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch (0);

			if ((touch.deltaPosition.magnitude > 15)  && detailRemoving == false) 
			{
				
				detailRemoving = true;
				ToAnimate.Add (removeUI);
			}
		}

		if (Application.isEditor) {
			if (Input.GetMouseButtonDown (1))
				hideUI (agent.currentNode);

		}
	}
}
