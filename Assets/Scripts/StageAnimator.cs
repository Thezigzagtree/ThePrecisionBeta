using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageAnimator : MonoBehaviour {

	public Image Correcthit;
	public Animator Correcthit_animator;
	public Image Damage;
	public Animator Damage_animator;
	public Image Blackscreen;
	public Animator Blackscreen_animator;
	public Image Outcome;
	public Animator Outcome_Animator;
	public Animator ReplayButton_animator;
	public Animator ExitButton_animator;
	public Animator Initbeg;
	private Animator StarAnimator;
	public Animator ReviveButtonAnimator;
	//public Image HitglowEffect;

	public Animator Hitglow_animator;

	private Image[] CorrecthitSet;

	private Vector3[] og_positions;
	//FOR DOUBLE TARGET STAGES

	private Vector3 origin = new Vector3 (540, 960, 0);

	void Awake()
	{
		Correcthit_animator.SetBool ("StageSetup", true);
		StarAnimator = GameObject.FindGameObjectWithTag ("Rank").GetComponent<Animator> ();
	}
		
	IEnumerator FirstCorrect()
	{
		Correcthit_animator.SetBool ("EndSetup", true);
		yield return new WaitUntil (() => Correcthit.transform.rotation == new Quaternion (359,359,359,1));
		ShowCorrectHit (origin);
		if (FindObjectOfType<AudioScript> () != null)
			FindObjectOfType<AudioScript> ().PlaySafeSFX ();
		FindObjectOfType<basic_stagemaster_functions> ().Switchon_Manipulator ();
		FindObjectOfType<basic_stagemaster_functions> ().Firstchoice ();

	}

	IEnumerator EndSetup()
	{
		Correcthit_animator.SetBool ("FinishSetup", true);
		yield return new WaitUntil (() => Correcthit.color.a == 0);
		StartCoroutine (FirstCorrect ());

		//ShowCorrectHit(new Vector3 (540, 960, 0));

	}

	public void FinishSetUp()
	{
		Correcthit_animator.SetBool ("StageSetup", false);
		HideDamage ();
		StartCoroutine (EndSetup ());

	}
		
	public void hpGainGlow()
	{
		Hitglow_animator.Play ("hpGainGlow");
	}

	public void Correcthit_glow()
	{
		Hitglow_animator.Play ("HitGlow");
	}
			
	public void LossButtons()
	{
		ReplayButton_animator.SetBool ("Lose", true);
		ExitButton_animator.SetBool ("End", true);
		if(PlayerPrefs.GetInt("revive") == 0)
		{
			ReviveButtonAnimator.SetBool ("End", true);
			//	ReviveButtonAnimator.GetComponent<Button> ().enabled = false;
		}


	}

	public void WinButtons()
	{

		ExitButton_animator.SetBool ("Win", true);
		ReplayButton_animator.SetBool ("Win", true);
	}

	public void RevealStars()
	{
		StarAnimator.SetBool ("Victory", true);
	}

	public void VictoryOutcome()
	{
		Outcome_Animator.SetBool ("Victory", true);
	}

	public void DefeatOutcome()
	{
		Outcome_Animator.SetBool ("Loss", true);
	}

	IEnumerator FadeToNextStage()
	{
		Blackscreen_animator.SetBool ("Fade", true);
		yield return new WaitUntil (() => Blackscreen.color.a == 1);
		SceneManager.LoadScene("Stage_" + ((PlayerPrefs.GetInt("Completed")+1).ToString("0000")));

	}

	IEnumerator FadeToMainMenu()
	{
		Blackscreen_animator.SetBool ("Fade", true);
		yield return new WaitUntil (() => Blackscreen.color.a == 1);
		SceneManager.LoadScene ("WorldMap");
	}

	IEnumerator FadeToSameStage(string name)
	{
		Blackscreen_animator.SetBool ("Fade", true);
		yield return new WaitUntil (() => Blackscreen.color.a == 1);
		SceneManager.LoadScene (name);
	}

	public void Replay(string name)
	{
		StartCoroutine (FadeToSameStage (name));
	}

	public void Exit ()	
	{
		if (GameObject.FindObjectOfType<AudioScript> () != null) 
			FindObjectOfType<AudioScript> ().GetComponent<AudioLowPassFilter> ().cutoffFrequency = 6000;
		if (GameObject.FindGameObjectWithTag("permaObject"))
			{
		GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().setEquippedItem ("");
			}
			StartCoroutine (FadeToMainMenu ());
	}

	public void ShowDamage()
	{
		
		Hitglow_animator.SetBool ("Safe", false);
		Damage_animator.SetBool ("Damage", true);

	}

	public void HideDamage()
	{
		Hitglow_animator.SetBool ("Safe", true);
		Damage_animator.SetBool ("Damage", false);
	}

	public void ShowDoubleHit()
	{
		StartCoroutine (DoubleHit_animation ());
	}

	IEnumerator DoubleHit_animation ()
	{
		Correcthit_animator.SetBool ("Double", true);
		yield return new WaitUntil (() => Correcthit.color.a == 0);
		Correcthit_animator.SetBool ("Double", false);
	}

	IEnumerator CorrectHit_animation(Vector3 loc)
	{
		Correcthit.transform.position = new Vector3(loc.x, loc.y, loc.z-0.1f);
		Correcthit.GetComponent<Image> ().color = new Color (1, 1, 20/255, 100);
		Correcthit.color = new Color (Correcthit.color.r, Correcthit.color.g, Correcthit.color.b, 100);
		Correcthit_animator.SetBool ("Correct", true);
		yield return new WaitUntil (() => Correcthit.color.a == 0);
		Correcthit_animator.SetBool ("Correct", false);
	}

	public void ShowCorrectHit(Vector3 loc)
	{
		StartCoroutine(CorrectHit_animation(loc));

	}

	public void LoadMainMenu()
	{
		StartCoroutine (FadeToMainMenu ());
	}

	public void LoadNextStage()
	{
		if (PlayerPrefs.GetInt ("Completed") < SceneManager.sceneCountInBuildSettings - 3)
			StartCoroutine (FadeToNextStage ());
		else
			StartCoroutine (FadeToMainMenu ());
	}

	public void checkReviveButton()
	{
		if (!FindObjectOfType<AdController>() || !FindObjectOfType<AdController> ().checkAdStatus())
			ReviveButtonAnimator.GetComponent<Button> ().interactable = false;
	}
}
