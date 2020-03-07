using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class MainscreenManager : MonoBehaviour {

	public Image Blackbox;
	public Animator anim;
	public Animator regFormAnimator;

	public Material mainScreenTargetMat;
	private ColorBlock tempcolor;


	public GameObject permaObject;
	public AudioSource Soundmanager;

	//public Button startbutton;
	//MOVE STAGEBUTTONS WITH TOUCH

	//OPTIONS BUTTONS


	public void userLoggedIn()
	{
		regFormAnimator.SetBool("LogIn", true);
	}

	public void shrinkRegForm()
	{
		regFormAnimator.SetBool("ExpandRegForm", false);
	}

	public void shrinkLoginForm()
	{
		regFormAnimator.SetBool("ExpandLoginForm", false);
	}

	public void expandRegForm()
	{
		regFormAnimator.SetBool("ExpandRegForm", true);
	}

	public void expandLoginForm()
	{
		regFormAnimator.SetBool("ExpandLoginForm", true);
	}



	IEnumerator FadeToQuit()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => Blackbox.color.a == 1);
		Application.Quit ();
	}


	IEnumerator FadeToNextStage()
	{
		if(GameObject.FindGameObjectWithTag("SoundManager"))
			GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>().GetComponent<AudioScript> ().PlayInitBegSFX ();
		
			anim.SetBool ("Fade", true);
			yield return new WaitUntil (() => Blackbox.color.a == 1);


		SceneManager.LoadScene ("WorldMap");

	}



	public void MusicSetup()
	{
			
			if (PlayerPrefs.GetInt ("MusicOn") == 1) {
				Soundmanager.GetComponent<AudioScript> ().StartPlaying ();
			} else
				Soundmanager.GetComponent<AudioSource> ().Pause();
		
	}
		
	void Awake()
	{
		SaveSystem.SetInt("position", 0);
		if (!PlayerPrefs.HasKey ("MusicOn"))
			PlayerPrefs.SetInt ("MusicOn", 1);
		if (!PlayerPrefs.HasKey ("HapticOn"))
			PlayerPrefs.SetInt ("HapticOn", 1);
		if (!PlayerPrefs.HasKey ("TrailRenderer"))
			PlayerPrefs.SetInt ("TrailRenderer", 1);
		if (!PlayerPrefs.HasKey ("HitGraphic"))
			PlayerPrefs.SetInt ("HitGraphic", 1);
		if (!PlayerPrefs.HasKey ("Credits"))
			PlayerPrefs.SetInt ("Credits", 0);
		if (!PlayerPrefs.HasKey ("redLight"))
			PlayerPrefs.SetFloat ("redlight", Random.Range (0, 1f));
		if (!PlayerPrefs.HasKey("greenLight"))
			PlayerPrefs.SetFloat ("greenlight", Random.Range (0, 1f));
		if (!PlayerPrefs.HasKey ("blueLight"))
			PlayerPrefs.SetFloat ("blueLight", Random.Range (0, 1f));
		

		if(GameObject.FindGameObjectsWithTag("permaObject").Length == 1)
			DontDestroyOnLoad (permaObject);
		else
			permaObject.SetActive(false);
			//GameObject.Destroy(permaObject);

		if(GameObject.FindGameObjectsWithTag("SoundManager").Length == 1)
			DontDestroyOnLoad (Soundmanager);
		else
			Soundmanager.gameObject.SetActive(false);


	}


	public void nextbutton_function()
	{
		StartCoroutine (FadeToNextStage ());
		//SceneManager.LoadScene("Stage_" + ((PlayerPrefs.GetInt("Completed")+1).ToString("0000")));

	}

	void Start()
	{
		GameObject Arena = Instantiate (Resources.Load ("Cores/Stage_" + Random.Range(4,29).ToString("0000")+"A")) as GameObject;
		for (int i = 0; i < Arena.GetComponentsInChildren<MeshRenderer> ().Length; i++) {
			Arena.GetComponentsInChildren<MeshRenderer> () [i].material = mainScreenTargetMat;
		}
		Arena.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
//		FindObjectOfType<playerObj> ().loadGame ();

	}

	void Update()
	{

		if (Input.GetKeyDown (KeyCode.Escape))
			StartCoroutine (FadeToQuit ());


			
	}
}