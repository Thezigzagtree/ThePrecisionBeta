using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageReport : MonoBehaviour {

	public Text creditText;
	public Text timeText;
	public Text percentText;
	public Text comboText;
	public Text firstText;
	public Text bonusItemText;
	public Text flowText;

	private int creditGain = 0;
	private bool itemRoll = false;
	void Awake()
	{
		checkBonus ();
	}

	public void addInitGain()
	{
		creditGain += FindObjectOfType<stageStats> ().initGain;
	}

	public void writeReport(float totalTime, float hpPercent, int maxCombo, float points)
	{
		addInitGain ();
		writeTime (totalTime);
		writePercent (hpPercent);
		writeCombo (maxCombo);
		writeFlow (points);
		writeCredit ();
		checkBonusItem (points);

	}

	public void writeFlow(float points)
	{
		flowText.text = points.ToString ("####");

	}

	public void writeCredit()
	{
		creditText.text = "+"+ creditGain.ToString ()+"\nCredits!";
		FindObjectOfType<playerObj> ().addCredits (creditGain);
	}

	public void writeTime(float totalTime)
	{
		
		timeText.text = totalTime.ToString ("F2");
		float timeBonus = FindObjectOfType<stageStats> ().GetStageGoal(FindObjectOfType<basic_stagemaster_functions> ().current_stage) - totalTime;

		if (timeBonus > 0) {
			int increase = Mathf.CeilToInt (Random.Range (1, timeBonus));
			timeText.text += "... +" + increase.ToString() + " Credits";
			creditGain += increase;

		}
	}

	public void writePercent(float hpPercent)
	{
		percentText.text = (100*hpPercent).ToString ("F1") + "%";
		if (hpPercent >= 0.5) {
			int increase = Random.Range (3, 5) + FindObjectOfType<stageStats>().initGain;
			percentText.text += "... +" + increase.ToString() + " Credits";
			creditGain += increase;
		}

		else if (hpPercent >= 0.25) {
			int increase = Random.Range (1, 3)+ + FindObjectOfType<stageStats>().initGain;
			percentText.text += "... +" + increase.ToString()+ " Credits";
			creditGain += increase;
		}

	}

	public void writeCombo (int maxCombo)
	{
		comboText.text = maxCombo.ToString ();

		int increase = Mathf.FloorToInt( maxCombo / 3);
		if (increase > 0)
			comboText.text += "... +" + increase.ToString () + " Credits";
			creditGain += increase;

	}

	public void checkBonus()
	{
		if (!FindObjectOfType<playerObj>().playerHasStage(FindObjectOfType<basic_stagemaster_functions>().current_stage)) 
		{
			firstText.text = "+10";
			creditGain += 10;
			itemRoll = true;
		}
	}

	public void checkBonusItem(float points)
	{
		if (itemRoll) {
			if (Random.Range (0, 100f) <= 1 + (points / 100)) {
				string itemName, itemDesc;

				FindObjectOfType<itemBank> ().getRandomItem (out itemName, out itemDesc);
				FindObjectOfType<playerObj> ().addItem (itemName);
				GameObject.FindGameObjectWithTag ("bonusItem").GetComponentInChildren<Text> ().text = "You have obtained";
				bonusItemText.text = itemName;
			}
		}
	}

}
