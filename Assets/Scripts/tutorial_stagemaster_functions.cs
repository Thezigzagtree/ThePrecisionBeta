using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorial_stagemaster_functions : MonoBehaviour {

	//NEED THIS FOR EACH STAGE


	public Image Blackbox;
	public Animator anim;
	public Image Damageburst;
	public Animator Damage_animator;

	public Image fader;

	IEnumerator FadeToMainMenu()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => Blackbox.color.a == 1);
		SceneManager.LoadScene ("WorldMap");
	}


	public void exit()
	{
		StartCoroutine (FadeToMainMenu ());
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			StartCoroutine (FadeToMainMenu ());
	}
}
