using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageStats : MonoBehaviour {

	/// <summary>
	/// Hitmode
	/// Speed_threshold
	/// Stagetimer
	/// Stage_goal
	/// HP_gain_quan.
	/// </summary>


	public string Hitmode;
	public int stageGoal;
	public int speedThreshold;
	public float stageTimer;
	public float stageHpGain;
	public int initGain = 0;
	public string stageNickname;



	public string GetStageNickname(string stagenickname)
	{
		return stageNickname;
	}

	public string GetHitMode(string stagename)
	{
		return Hitmode;
	}

	public int GetSpeedThreshold(string stagename)
	{
		return speedThreshold;
	}

	public float GetStageTimer (string stagename)
	{
		return stageTimer;
	}

	public int GetStageGoal (string stagename)
	{
		return stageGoal;
	}

	public float GetStageHpGainQuan (string stagename)
	{
		return stageHpGain;
	}

	public void reduceStageGoal(int red)
	{
		stageGoal -= red;
	}
}
