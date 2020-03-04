using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevealOptions : MonoBehaviour {

	public GameObject InGameOptionsMenu;
	private List<System.Action> ToAnimate = new List<System.Action>();
	private Vector3 DampVelocity;
	public bool Optionsrevealed = false;
	private Vector3 Option_hidelocation = new Vector3 (-0,-800,0);
	private Vector3 Option_reveallocation = new Vector3 (0,-400,0);
	public Button MusicButton;
	public Button HapticButton;
	public Sprite hapticOn;
	public Sprite hapticOff;
	public Sprite musicOn;
	public Sprite musicOff;

	void Start()
	{
		if (PlayerPrefs.GetInt ("MusicOn") == 1) {
			MusicButton.GetComponent<Image> ().sprite = musicOff;
			Debug.Log ("MusicOn");
		} else {
			Debug.Log ("MusicOff");
			MusicButton.GetComponent<Image> ().sprite = musicOn;

		}
		if (PlayerPrefs.GetInt ("HapticOn") == 1) {
			HapticButton.GetComponent<Image> ().sprite = hapticOff;
			Debug.Log ("HapticOn");
		} else {
			HapticButton.GetComponent<Image> ().sprite = hapticOn;
			Debug.Log ("HapticOff");
		}

		

	}

	public void AddHideOptions()
	{
		ToAnimate.Add (HideOptions);
	}

	public void OptionsButton()
	{ 	
		if (!ToAnimate.Contains (ShowOptions) && !ToAnimate.Contains (HideOptions)) 
		{
			if (!Optionsrevealed)
				ToAnimate.Add (ShowOptions);
			else
				ToAnimate.Add (HideOptions);
		}

	}

	// Update is called once per frame
	void Update () 
	{
		
		for (int i = 0; i < ToAnimate.Count; i++) {
			ToAnimate [i]();
		}	
			
	}

	public void HideOptions()
	{
		InGameOptionsMenu.transform.localPosition = Vector3.SmoothDamp(InGameOptionsMenu.transform.localPosition, Option_hidelocation, ref DampVelocity, 0.15f);
		if (Vector3.Distance (InGameOptionsMenu.transform.localPosition, Option_hidelocation) < 0.1) 
		{
			Optionsrevealed = false;
			ToAnimate.Remove (HideOptions);
		}
	}

	public void ShowOptions()
	{
		InGameOptionsMenu.transform.localPosition = Vector3.SmoothDamp(InGameOptionsMenu.transform.localPosition, Option_reveallocation, ref DampVelocity, 0.15f);
		if (Vector3.Distance (InGameOptionsMenu.transform.localPosition, Option_reveallocation) < 0.1) 
		{
			Optionsrevealed = true;
			ToAnimate.Remove (ShowOptions);
		}
	}

	public void ToggleMusicButton()
	{
		if (PlayerPrefs.GetInt("MusicOn") == 1) 
		{
			PlayerPrefs.SetInt ("MusicOn", 0);
			MusicButton.GetComponent<Image> ().sprite = musicOn;
			GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>().Pause ();
		}

		else 
		{
			PlayerPrefs.SetInt ("MusicOn", 1);
			MusicButton.GetComponent<Image> ().sprite = musicOff;
			GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioScript>().StartPlaying ();

		}
	}
		
	public void ToggleHapticButton()
	{
		if (PlayerPrefs.GetInt ("HapticOn") == 1) {
			PlayerPrefs.SetInt ("HapticOn", 0);
			HapticButton.GetComponent<Image> ().sprite = hapticOn;
		} else {
			PlayerPrefs.SetInt ("HapticOn", 1);
			HapticButton.GetComponent<Image> ().sprite = hapticOff;
		}
	}

}




