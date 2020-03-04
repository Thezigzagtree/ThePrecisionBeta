using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuButtonText : MonoBehaviour {

	public Text MusicToggle;

	void Start()
	{
		if (PlayerPrefs.GetInt ("MusicOn") == 1) {
			MusicToggle.text = "Music Off";
		} else
			MusicToggle.text = "Music On";
	}

}
