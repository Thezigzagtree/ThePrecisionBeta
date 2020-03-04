using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JSNodeMap;

public class updateButtonText : MonoBehaviour {
	void Awake () {
		
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Agent>().OnMoveEnd += updateStagePlayButton;
	}

	void updateStagePlayButton (Node targetNode) {
			GetComponent<Text> ().text = "Start " + targetNode.name;

	}
}