using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemBank : MonoBehaviour {

	private float moveSpeed = 1;

	IEnumerator slowWave(float t = 0.25f, float s = 2)
	{
		setMoveSpeed (t);
		yield return new WaitForSeconds (s);
		setMoveSpeed (1);
	}


	public float getMoveSpeed()
	{
		return moveSpeed;
	}


	public void getRandomItem(out string itemName, out string itemDesc)
	{
		int powerCount = itemList.Count;
		int abilityCount = inGameItemList.Count;

		int selection = Random.Range (0, powerCount + abilityCount);

		if (selection > powerCount) {
			itemName = inGameItemList [selection - powerCount].getItemName ();
			itemDesc = inGameItemList [selection - powerCount].getItemDescription ();
		
		} else {
			itemName = itemList [selection].getItemName ();
			itemDesc = itemList [selection].getItemDescription ();
		
		}
	}



	private Dictionary <string, System.Action> itemFunctions = new Dictionary<string, System.Action> ();
	private List<bankPowerUpItem> itemList = new List<bankPowerUpItem>();
	private List<bankPowerUpItem> inGameItemList = new List<bankPowerUpItem>();

	public void setMoveSpeed(float s)
	{
		moveSpeed = s;
	}


	public class bankPowerUpItem
	{
		public string itemName;
		public string itemDescription;
		public int itemCost;
		public string itemType;


		public bankPowerUpItem(string name, string desc, int cost, string type, List<bankPowerUpItem> itemList)
		{
			itemName = name;
			itemDescription = desc;
			itemCost = cost;
			itemType = type;
			itemList.Add (this);

		}

		public string getItemName()
	{
			return itemName;
	}

		public string getItemDescription()
	{
			return itemDescription;
	}

		public string getCostasString()
		{
			return itemCost.ToString ();
		}

		public string getItemCostType()
	{
			return itemType;
	}

	}

	//private Dictionary<string, powerUpItem> itemList = new Dictionary<string, powerUpItem>();

	void Awake()
	{	
		//PlayerPrefs.SetInt ("Credits", 1000);
		populateItemList ();
		populateItemFunctions ();
	}


	public void checkShapeUnlocks()
	{
		for (int i = 0; i < Resources.LoadAll ("AgentShapes").Length; i++) 
		{
			string shape = Resources.LoadAll ("AgentShapes") [i].name;

			if (FindObjectOfType<playerObj>().agentShapes[shape] < 1)
			{
				if (shape == "Arkus") 
				{
					if (FindObjectOfType<sceneryManager> ().checkSceneryUnlocked ())
						//FindObjectOfType<playerObj> ().agentShapes [shape] = 1;
						SaveSystem.SetInt(shape, 1);
					//MainManager checks if scenery is all unlocked
				}

				else if (shape == "Spire") {
					if (FindObjectOfType<WorldMapManager> ().starCount >= 500)
						SaveSystem.SetInt(shape, 1);
					//check Star rank
				}

				else if (shape == "Waystone") {
					int x = 0;
					for(int j = 0; j < Resources.LoadAll("Cores/").Length;j++)
					{
						if (FindObjectOfType<playerObj>().getStageRank(Resources.LoadAll ("Cores/") [j].name) >= 3)
							x += 1;
					}
					if (x >= 100)
						SaveSystem.SetInt(shape, 1);
					
					//check star ranking with 3
				}

				else if (shape == "Battery") {
					if (FindObjectOfType<playerObj>().getStageRank("Stage_0120A") > 0 && 
					FindObjectOfType<playerObj>().getStageRank("Stage_0121A") > 0 && 
					FindObjectOfType<playerObj>().getStageRank("Stage_0122A") > 0 && 
					FindObjectOfType<playerObj>().getStageRank("Stage_0123A") > 0 &&
					FindObjectOfType<playerObj>().getStageRank("Stage_0124A") > 0 &&
					FindObjectOfType<playerObj>().getStageRank("Stage_0125A") > 0)
					{
						SaveSystem.SetInt(shape, 1);
					}
				}
			}
		}
	}

	public void getLockCondition(string shape)
	{
		Debug.Log (shape);
		if (shape == "Arkus")
			GameObject.FindGameObjectWithTag ("conditionText").GetComponent<Text> ().text = "Unlock All Landmarks";
			//Condition
		else if (shape == "Spire")
			GameObject.FindGameObjectWithTag ("conditionText").GetComponent<Text> ().text = "Obtain 500 Stars";
			//Condition
		else if(shape == "Waystone")
			GameObject.FindGameObjectWithTag ("conditionText").GetComponent<Text> ().text = "Obtain 3 Star Ranking in 100 Stages";
			//Condition
		else if (shape == "Battery")
			GameObject.FindGameObjectWithTag ("conditionText").GetComponent<Text> ().text = "Finish Stages 120A-125A";
	}


	public void populateItemList()
	{

		//INCREASES STARTING HP
		bankPowerUpItem healthLv1 = new bankPowerUpItem ("Health Lv1", "Start the round with an extra 1/3 of HP", 100, "Credits", itemList);
		bankPowerUpItem healthLv2 = new bankPowerUpItem ("Health Lv2", "Start the round with an extra 2/3 of extra HP", 350, "Credits", itemList);
		bankPowerUpItem healthLv3 = new bankPowerUpItem ("Health Lv3", "Start the round with a full bar of extra HP", 1, "silverCredits", itemList);


		//INCREASES HIT RADIUS
		bankPowerUpItem wideRangeLv1 = new bankPowerUpItem ("Wide Range Lv1", "Increase hit detection radius a tiny bit", 150, "Credits", itemList);
		bankPowerUpItem wideRangeLv2 = new bankPowerUpItem ("Wide Range Lv2", "Increase hit detection radius a little", 300, "Credits", itemList);
		bankPowerUpItem wideRangeLv3 = new bankPowerUpItem ("Wide Range Lv3", "Increase hit detection radius", 1, "silverCredits", itemList);

		//INCREASES HP GAIN QUAN
		bankPowerUpItem restoreLv1 = new bankPowerUpItem ("Restore Lv1", "Slightly increases HP gain when under half HP", 80, "Credits", itemList);
		bankPowerUpItem restoreLv2 = new bankPowerUpItem ("Restore Lv2", "Doubles HP gain quantity when under half HP", 250, "Credits", itemList);

		//10

		//REDUCES STAGE GOAL
		bankPowerUpItem advantageLv1 = new bankPowerUpItem ("Advantage Lv1", "Reduces Stage Goal by 3", 100, "Credits", itemList);
		bankPowerUpItem advantageLv2 = new bankPowerUpItem ("Advantage Lv2", "Reduces Stage Goal by 6", 250, "Credits", itemList);
		bankPowerUpItem advantageLv3 = new bankPowerUpItem ("Advantage Lv3", "Reduces Stage Goal by 10", 600, "Credits", itemList);

		//INCREASES TIME IN BETWEEN HITS
		bankPowerUpItem heavyhitLv1 = new bankPowerUpItem ("Heavy Hit Lv1", "Slightly increases allowed time in between hits", 50, "Credits", itemList);
		bankPowerUpItem heavyhitLv2 = new bankPowerUpItem ("Heavy Hit Lv2", "Increases allowed time in between hits", 200, "Credits", itemList);
		bankPowerUpItem heavyhitLv3 = new bankPowerUpItem ("Heavy Hit Lv3", "Drastically increases allowed time in between hits", 1, "silverCredits", itemList);

		//REDUCES TOTAL DECREASE

		bankPowerUpItem StabilityLv1 = new bankPowerUpItem ("Stability Lv1", "Slightly reduces how much HP you can lose at a time", 75, "Credits", itemList);
		bankPowerUpItem StabilityLv2 = new bankPowerUpItem ("Stability Lv2", "Reduces how much HP you can lose at a time", 150, "Credits", itemList);
		bankPowerUpItem StabilityLv3 = new bankPowerUpItem ("Stability Lv3", "Significantly reduces how much HP you can lose at a time", 200, "Credits", itemList);

		//IN GAME POWER UP SECTION

		//SLOW DOWN TIME
		bankPowerUpItem slowLv1 = new bankPowerUpItem ("Slow Lv1", "Slow down time for a short period", 150, "Credits", inGameItemList);
		bankPowerUpItem slowLv2 = new bankPowerUpItem ("Slow Lv2", "Slow down time for a while", 500, "Credits", inGameItemList);

		bankPowerUpItem healLv1 = new bankPowerUpItem ("Heal Lv1", "Heal a quarter of your HP", 150, "Credits", inGameItemList);
		bankPowerUpItem healLv2 = new bankPowerUpItem ("Heal Lv2", "Heal half of your HP", 300, "Credits", inGameItemList);
		bankPowerUpItem healLv3 = new bankPowerUpItem ("Heal Lv3", "Heal your full HP", 1, "silverCredits", inGameItemList);

		bankPowerUpItem shrinkLv1 = new bankPowerUpItem ("Shrink Lv1", "Make incorrect targets slightly smaller", 100, "Credits", inGameItemList);
		bankPowerUpItem shrinkLv2 = new bankPowerUpItem ("Shrink Lv2", "Make incorrect targets slightly smaller for a longer period of time.", 250, "Credits", inGameItemList);

		bankPowerUpItem SnipeLv1 = new bankPowerUpItem ("Snipe Lv1", "Counts as a correct hit.", 50, "Credis", inGameItemList);
	}

	public Color getItemColor(string itemName)
	{
		Debug.Log (checkPowerUpType (itemName));
		Debug.Log (checkPowerUpCreditType (itemName));
		if (checkPowerUpType (itemName) == "inGame" && checkPowerUpCreditType (itemName) == "Credits") 
		{
			return new Color(228/255f, 119/255f, 234/255f);
		}

		else if (checkPowerUpType (itemName) == "inGame" && checkPowerUpCreditType (itemName) == "silverCredits") {
			return new Color(16/255f, 26/255f, 92/255f);
		}

		else if (checkPowerUpType (itemName) == "preGame" && checkPowerUpCreditType (itemName) == "Credits") {
			return new Color(119/255f, 191/255f, 234/255f);
		}

		else if (checkPowerUpType (itemName) == "preGame" && checkPowerUpCreditType (itemName) == "silverCredits") {
			return new Color(234/255f, 136/255f, 119/255f);
		}


		return new Color(0,0,0);
	}

	public string checkPowerUpCreditType (string itemName)
	{
		foreach (bankPowerUpItem item in itemList) {
			if (item.itemName == itemName)
				return item.getItemCostType();
		}

		foreach (bankPowerUpItem item in inGameItemList) {
			if (item.itemName == itemName)
				return item.getItemCostType();
		}

		return "";
		
	}

	public string checkPowerUpType (string itemName)
	{
		foreach (bankPowerUpItem item in itemList) {
			if (item.itemName == itemName)
				return "preGame";
		}

		foreach (bankPowerUpItem item in inGameItemList) {
			if (item.itemName == itemName)
				return "inGame";
		}

		return "";
	}

	public List<bankPowerUpItem> getItemList()
	{
		return itemList;
	}

	public List<bankPowerUpItem> getInGameItemList()
	{
		return inGameItemList;
	}
		
	public void populateItemFunctions()
	{
		itemFunctions.Add ("Health Lv1", bonusHealthLv1);
		itemFunctions.Add ("Health Lv2", bonusHealthLv2);
		itemFunctions.Add ("Health Lv3", bonusHealthLv3);

		itemFunctions.Add ("Wide Range Lv1", increaseHitRadiusLv1);
		itemFunctions.Add ("Wide Range Lv2", increaseHitRadiusLv2);
		itemFunctions.Add ("Wide Range Lv3", increaseHitRadiusLv3);

		itemFunctions.Add ("Restore Lv1", restoreLv1);
		itemFunctions.Add ("Restore Lv2", restoreLv2);

		// 10

		itemFunctions.Add ("Advantage Lv1", advantageLv1);
		itemFunctions.Add ("Advantage Lv2", advantageLv2);
		itemFunctions.Add ("Advantage Lv3", advantageLv3);

		itemFunctions.Add ("Heavy Hit Lv1", heavyHitLv1);
		itemFunctions.Add ("Heavy Hit Lv2", heavyHitLv2);
		itemFunctions.Add ("Heavy Hit Lv3", heavyHitLv3);

		itemFunctions.Add ("Slow Lv1", slowLv1);
		itemFunctions.Add ("Slow Lv2", slowLv2);

		itemFunctions.Add ("Heal Lv1", healLv1);
		itemFunctions.Add ("Heal Lv2", healLv2);
		itemFunctions.Add ("Heal Lv3", heallv3);

		itemFunctions.Add ("Shrink Lv1", shrinkLv1);
		itemFunctions.Add ("Shrink Lv2", shrinkLv2);

		itemFunctions.Add ("Snipe Lv1", snipeLv1);
	}
		

	public void findItemEffect(string usedItem)
	{

		itemFunctions [usedItem] ();
		FindObjectOfType<playerObj> ().useItem (usedItem);
		GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().setEquippedItem ("");
		if (checkPowerUpType (usedItem) == "inGame")
			FindObjectOfType<basic_stagemaster_functions> ().init_shrinkPowerUpButton ();

	}
		
	//Continuous functions should just pass through.
	//Usable Items Should Generate the Button that uses them as well
	public void bonusHealthLv1()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setBonusHp (3.3f);
	}

	public void bonusHealthLv2()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setBonusHp (6.6f);
	}

	public void bonusHealthLv3()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setBonusHp (10);
	}

	public void increaseHitRadiusLv1()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setSphereCastRadius (0.038f);
	}

	public void increaseHitRadiusLv2()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setSphereCastRadius (0.042f);
	}

	public void increaseHitRadiusLv3()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setSphereCastRadius (0.045f);
	}

	public void restoreLv1()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setHPGainQuan (0.10f);
	}

	public void restoreLv2()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setHPGainQuan (0.25f);
	}

	public void advantageLv1()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setStageGoal (3);
	}

	public void advantageLv2()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setStageGoal (6);
	}
		
	public void advantageLv3()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setStageGoal (10);
	}

	public void heavyHitLv1()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setStageTimer (0.3f);
	}

	public void heavyHitLv2()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setStageTimer (0.6f);
	}
	public void heavyHitLv3()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setStageTimer (1f);
	}

	public void stabilityLv1()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setTotalDecrease (4.0f);
	}
		
	public void stabilityLv2()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setTotalDecrease (3.5f);
	}

	public void stabilityLv3()
	{
		FindObjectOfType<basic_stagemaster_functions> ().setTotalDecrease (3.0f);
	}


	public void slowLv1()
	{
		StartCoroutine (slowWave (0.25f, 5));	
	}


	public void slowLv2()
	{
		StartCoroutine (slowWave (0.25f, 8));	
	}

	public void healLv1()
	{
		FindObjectOfType<basic_stagemaster_functions> ().increaseCurrentHealth(FindObjectOfType<basic_stagemaster_functions> ().getMaxHealth() / 4);
	}

	public void healLv2()
	{
		FindObjectOfType<basic_stagemaster_functions> ().increaseCurrentHealth(FindObjectOfType<basic_stagemaster_functions> ().getMaxHealth() / 2);
	}

	public void heallv3()
	{
		FindObjectOfType<basic_stagemaster_functions> ().increaseCurrentHealth(FindObjectOfType<basic_stagemaster_functions> ().getMaxHealth());
	}

	public void shrinkLv1()
	{
		FindObjectOfType<basic_stagemaster_functions> ().init_shrinkWave (5);		
	}

	public void shrinkLv2()
	{
		FindObjectOfType<basic_stagemaster_functions> ().init_shrinkWave (10);		
	}

	public void snipeLv1()
	{
		FindObjectOfType<basic_stagemaster_functions> ().snipeShot ();
	}
}