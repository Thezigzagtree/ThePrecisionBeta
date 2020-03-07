using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerObj : MonoBehaviour {

	
	public int credits = 0;
	public int silverCredits = 0;
	public int location = 0;
	public string currentAgentShape;
	public Dictionary <string, int> agentShapes = new Dictionary<string, int>();
	public Material agentMat1;
	public Material agentMat2;
	//KEEP TRACK OF RANK AND STAGE
	

	public void addItem(string item)
	{

		if (SaveSystem.inventoryItems.ContainsKey (item))
			
			SaveSystem.inventoryItems [item] += 1;
		else
			SaveSystem.inventoryItems.Add (item, 1);

		FindObjectOfType<FirebaseObject>().saveItems(item, SaveSystem.inventoryItems [item]);
		
	}

	public void useItem(string item)
	{
		
		SaveSystem.inventoryItems [item] -= 1;
		FindObjectOfType<FirebaseObject>().saveItems(item, SaveSystem.inventoryItems [item]);
	}
		
	public bool hasItem(string item)
	{
		if (SaveSystem.inventoryItems.ContainsKey (item))
			return true;
		else
			return false;

	}

	public int getItemCount(string item)
	{
		
		if(SaveSystem.inventoryItems.ContainsKey(item))
		{	if (SaveSystem.inventoryItems[item]> 0)
				return SaveSystem.inventoryItems[item];
			else {
				//SaveSystem.Remove(item);
				return 0;
			}

		} else
			return 0;
	}

	public void setupAgentShapes()
	{
		for (int i =0; i < Resources.LoadAll("AgentShapes").Length; i++)
		{
			if (Resources.LoadAll ("AgentShapes") [i].name == "Rings" || Resources.LoadAll ("AgentShapes") [i].name == "Cage")
				//agentShapes.Add (Resources.LoadAll ("AgentShapes") [i].name, 1);
				SaveSystem.SetInt(Resources.LoadAll ("AgentShapes") [i].name, 1);
			else
				//agentShapes.Add (Resources.LoadAll ("AgentShapes") [i].name, 0);
				SaveSystem.SetInt(Resources.LoadAll ("AgentShapes") [i].name, 0);
		}

		//SaveSystem.SetInt("Tutorial", 0);
			
	}

	public void uploadStage(string stagename, int rank)
	{
		SaveSystem.SetInt(stagename, rank);
		
		if (SaveSystem.stageToRank.ContainsKey (stagename))
			SaveSystem.stageToRank [stagename] = rank;
			
		else
			SaveSystem.stageToRank.Add (stagename, rank);
		
		FindObjectOfType<FirebaseObject>().saveStageToRank (stagename, rank);
	}

	public int getStageRank(string stagename)
	{
		if (SaveSystem.stageToRank.ContainsKey(stagename))
			return SaveSystem.stageToRank[stagename];//stageToRank[stagename];
		else
			return -1;
	}

	public bool playerHasStage(string stagename)
	{
		if (SaveSystem.stageToRank.ContainsKey(stagename))
			return true;
		else
			return false;
	}

	public void setLocation(int pos)
	{
		//location = pos;
		SaveSystem.SetInt("position", pos);
	}

	public int getLocation()
	{
		return location;
	}


	public void addCredits(int gain)
	{
		//credits += gain;
		SaveSystem.SetInt("credits", SaveSystem.GetInt("credits")+gain);
		FindObjectOfType<FirebaseObject>().saveCredits();
	}

	public void spendCredits(int cost)
	{
		
		SaveSystem.SetInt("credits", SaveSystem.GetInt("credits")-cost);
		FindObjectOfType<FirebaseObject>().saveCredits();
	}

	public void spendsilverCredits (int cost)
	{
		
		SaveSystem.SetInt("silverCredits", SaveSystem.GetInt("silverCredits")-cost);
		FindObjectOfType<FirebaseObject>().saveSilverCredits();
	}

	public void addSilverCredits(int gain)
	{
		//silverCredits += gain;
		SaveSystem.SetInt("silverCredits", SaveSystem.GetInt("silverCredits")+gain);
		FindObjectOfType<FirebaseObject>().saveSilverCredits();
	}

	public void saveGame()
	{
		//saveSystem.SaveGame (this);
		
	}

	public void loadGame()
	{
			credits = SaveSystem.GetInt("credits");
			silverCredits = SaveSystem.GetInt("silverCredits");
			location = SaveSystem.GetInt("position");
			currentAgentShape = SaveSystem.GetString("currentAgentShape");
				// SaveSystem.SetString("currentAgentShape", "Rings");
			
	}
		//agentShapes = data.agentShapes;
			//inventoryItems = data.inventoryItems;
			

		//KEEP TRACK OF RANK AND STAGE
		//public Dictionary <string, int> stageToRank = new Dictionary<string, int>();

	public void setShape(string shape)
	{
		SaveSystem.SetString("currentAgentShape", shape);
		FindObjectOfType<FirebaseObject>().saveAgentShape();
	}

	
	public void setupNewUser()
	{
		SaveSystem.SetInt("credits", 0);
		SaveSystem.SetInt("silverCredits", 0);
		SaveSystem.SetString("currentAgentShape", "Ring");
		uploadStage("Tutorial", 0);
	}

	public void SetupAgentAura(string r, string g, string b)
	{
		Debug.Log(int.Parse(r));
		Debug.Log(int.Parse(g));
		Debug.Log(int.Parse(b));
	}

	void Awake()
	{
		Application.targetFrameRate = 60;
		setupAgentShapes();

		//loadGame ();
	}
}
