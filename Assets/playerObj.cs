using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerObj : MonoBehaviour {

	
	public int credits = 0;
	public int silverCredits = 0;
	public int location = 0;
	public string currentAgentShape;
	//KEEP TRACK OF RANK AND STAGE
	public Dictionary <string, int> stageToRank = new Dictionary<string, int>();
	public Dictionary <string, int> agentShapes = new Dictionary<string, int> ();
	public Dictionary <string, int> inventoryItems = new Dictionary<string, int> ();


	public void addItem(string item)
	{

		SaveSystem.SetInt(item, SaveSystem.GetInt(item)+1);
		saveGame ();
		//if (inventoryItems.ContainsKey (item))
			
		//	inventoryItems [item] += 1;
		//else
		//	inventoryItems.Add (item, 1);


		//saveGame ();
		
	}

	public void useItem(string item)
	{
		SaveSystem.SetInt(item, SaveSystem.GetInt(item)-1);

		
	//	inventoryItems [item] -= 1;
//		if(SaveSystem.GetInt(item) == 0)
//			SaveSystem.Remove(item);
	//	if (inventoryItems [item] == 0)
	//		inventoryItems.Remove (item);
		saveGame ();
	}
		
	public bool hasItem(string item)
	{
		if(SaveSystem.HasKey(item))
		//if (inventoryItems.ContainsKey (item))
			return true;
		else
			return false;

	}

	public int getItemCount(string item)
	{
		
		if(SaveSystem.HasKey(item))
		{	if (SaveSystem.GetInt(item)> 0)
				return SaveSystem.GetInt(item);
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

		SaveSystem.SetInt("Tutorial", 0);
			
	}

	public void uploadStage(string stagename, int rank)
	{
		SaveSystem.SetInt(stagename, rank);
		
		//if (stageToRank.ContainsKey (stagename))
			//stageToRank [stagename] = rank;
			
		//else
		//	stageToRank.Add (stagename, rank);
		
		saveGame ();
	}

	public int getStageRank(string stagename)
	{
		if (SaveSystem.HasKey(stagename))
			return SaveSystem.GetInt(stagename);//stageToRank[stagename];
		else
			return -1;
	}

	public bool playerHasStage(string stagename)
	{
		if (SaveSystem.HasKey(stagename))
			return true;
		else
			return false;
	}

	public void setLocation(int pos)
	{
		//location = pos;
		SaveSystem.SetInt("position", pos);
		saveGame ();
	}

	public int getLocation()
	{
		return location;
	}

	public void addCredits(int gain)
	{
		//credits += gain;
		SaveSystem.SetInt("credits", SaveSystem.GetInt("credits")+gain);
		saveGame ();
	}

	public void spendCredits(int cost)
	{
		//credits -= cost;
		SaveSystem.SetInt("credits", SaveSystem.GetInt("credits")-cost);
		saveGame ();
	}

	public void spendsilverCredits (int cost)
	{
//		silverCredits -= cost;
		SaveSystem.SetInt("silverCredits", SaveSystem.GetInt("silverCredits")-cost);
		saveGame ();
	}

	public void addSilverCredits(int gain)
	{
		//silverCredits += gain;
		SaveSystem.SetInt("silverCredits", SaveSystem.GetInt("silverCredits")+gain);
		saveGame ();
	}

	public void saveGame()
	{
		//saveSystem.SaveGame (this);
		SaveSystem.SaveToDisk();
	}

	public void loadGame()
	{
			credits = SaveSystem.GetInt("credits");
			silverCredits = SaveSystem.GetInt("silverCredits");
			location = SaveSystem.GetInt("position");
			//stageToRank = data.stageToRank;
			if(!SaveSystem.HasKey("currentAgentShape"))
			{
				setupAgentShapes ();
				SaveSystem.SetString("currentAgentShape", "Rings");
			
			}

			currentAgentShape = SaveSystem.GetString("currentAgentShape");
			//agentShapes = data.agentShapes;
			//inventoryItems = data.inventoryItems;
			
			
			
	}

		//KEEP TRACK OF RANK AND STAGE
		//public Dictionary <string, int> stageToRank = new Dictionary<string, int>();

	


	public void setShape(string shape)
	{
		SaveSystem.SetString("currentAgentShape", shape);
		saveGame();
	}
	void Awake()
	{
		Application.targetFrameRate = 60;
		loadGame ();
	}
}
