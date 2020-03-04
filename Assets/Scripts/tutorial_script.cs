using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class tutorial_script : MonoBehaviour {


	// Use this for initialization
	public GameObject[] targets  = new GameObject[7];
	public GameObject[] tutorial_mes = new GameObject[5];
	public GameObject stagemaster;
	private int count = 0;

	void Awake() 
	{
		for (int i = 1; i < tutorial_mes.Length; i++) 
		{
			tutorial_mes [i].GetComponent<Text> ().enabled = false;
		}
	}
		
	void Update()
	{
		if (Input.touchCount > 0) 
		{
			Touch touch = Input.GetTouch (0);

			if (touch.phase == TouchPhase.Began && count < 5)
			{
				tutorial_mes [count].GetComponent<Text> ().enabled = false;
				count += 1;
				if (count < 5) {
					tutorial_mes [count].GetComponent<Text> ().enabled = true;
					if (count == 3)
						GetComponent<tutorial_stagemaster_functions> ().Damage_animator.SetBool ("Damage", true);
				}
				if (count == 4) {
					GetComponent<tutorial_stagemaster_functions> ().Damage_animator.SetBool ("Damage", false);
				}
					
		
			
			}

			if (count == 5) {
				FindObjectOfType<tutorial_stagemaster_functions> ().exit ();
			}
		}
	}

}