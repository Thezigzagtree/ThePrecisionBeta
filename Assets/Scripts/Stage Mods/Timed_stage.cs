using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timed_stage : MonoBehaviour {


	public float total_stagetime;

	void FixedUpdate()
	{
		if (FindObjectOfType<basic_stagemaster_functions> ().lost != true && FindObjectOfType<basic_stagemaster_functions> ().victory != true && FindObjectOfType<basic_stagemaster_functions> ().score >= 1) {
			FindObjectOfType<basic_stagemaster_functions> ().timertext.text = (total_stagetime + FindObjectOfType<basic_stagemaster_functions> ().startTime- Time.timeSinceLevelLoad).ToString ("F2");
		}
		if(Time.timeSinceLevelLoad > total_stagetime && FindObjectOfType<basic_stagemaster_functions> ().lost !=true && FindObjectOfType<basic_stagemaster_functions> ().victory != true)
		{
			FindObjectOfType<basic_stagemaster_functions> ().timertext.text = ("");
			FindObjectOfType<basic_stagemaster_functions> ().gameover ();
			FindObjectOfType<basic_stagemaster_functions> ().outcometext.fontSize = 60;
			FindObjectOfType<basic_stagemaster_functions> ().outcometext.text = "Time Out!";
			FindObjectOfType<basic_stagemaster_functions> ().outcometext.color = Color.cyan;
		}
	}
}
