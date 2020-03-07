using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using JSNodeMap;


public class WorldMapManager : MonoBehaviour {

public Agent agent;

	[HideInInspector]
	public int starCount = 0;

	static private AudioSource Soundmanager_reference;
	public Camera Maincam;
	public GameObject MapNodes;
	public Animator anim;
	public Image SceneFader;
	static private string curStage;
	public Text TrailButtonText;
	public Text MusicToggleText;
	public Text HapticText;
	public Text HitGraphicToggleText;

	public Material goldenMaterial;

	public Material AccessibleMat;
	public Material BlockedMat;

	private SpriteRenderer meshRend;
	private LineRenderer pathRend;

	public void deleteKeys()
	{
		PlayerPrefs.DeleteAll ();
	}

	void Awake()
	{
		FindObjectOfType<playerObj> ().loadGame ();	
		PlayerPrefs.SetInt ("revive", 0);
		PlayerPrefs.SetInt ("reviveScore", 0);
		Maincam.transform.position = new Vector3 (-7.5f, 30, -17.5f);
		agent.transform.position = MapNodes.transform.GetChild (SaveSystem.GetInt("position")).transform.position;//FindObjectOfType<playerObj>().getLocation()).transform.position;
		agent.currentNode = MapNodes.transform.GetChild (SaveSystem.GetInt("position")).GetComponent<Node> ();
		PlayerPrefs.SetString ("stageToLoad", MapNodes.transform.GetChild (SaveSystem.GetInt("position")).GetComponent<Node> ().name);

		if (GameObject.FindGameObjectWithTag("SoundManager"))
			Soundmanager_reference = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
		
		HapticButtonText ();
		MusicButtonText ();
		TrailRendererText ();
		HitGraphicText ();
		openUpFinishedPaths ();
		//Debug.Log ("Star Count : " + starCount);
		GameObject.FindGameObjectWithTag ("StarCount").GetComponentInChildren<Text> ().text = starCount.ToString ();
		//openUpAllPaths ();
		agent.OnMoveEnd += trackNode;
		agent.OnMoveStart += PlayNodeGong;

		FindObjectOfType<FollowObject> ().init_CenterToAgent ();

	}

	void PlayNodeGong(Node targetNode)
	{
		if (GameObject.FindGameObjectWithTag("SoundManager") != null)
			GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioScript>().PlayMovementSound();
	}

	public void playMenuStartSFX()
	{
		if (GameObject.FindGameObjectWithTag("SoundManager") != null)
			Soundmanager_reference.GetComponent<AudioScript> ().playMenuStartSFX ();
	}

	public void playMenuSFX()
	{
		if (GameObject.FindGameObjectWithTag("SoundManager") != null)
			Soundmanager_reference.GetComponent<AudioScript> ().playMenuSFX ();
	}

	public void playBacksFX()
	{
		if(GameObject.FindGameObjectWithTag("SoundManager") != null)
			FindObjectOfType<AudioScript> ().PlayBackSFX ();
	}


	public void skipStage()
	{
		if (!FindObjectOfType<playerObj>().playerHasStage(curStage) )
		{
			FindObjectOfType<playerObj>().spendCredits(300);
			FindObjectOfType<playerObj> ().uploadStage (curStage, 0);
			UnlockCompletedStages ();
			colorBlockedAndEndPoints ();
			FindObjectOfType<StageDetailToggle> ().initHideStageDetail ();
		}
	}

	public bool stageSkippable()
	{
		if (FindObjectOfType<playerObj>().playerHasStage(curStage))
			return false;
		else
			return true;
	}

	public void openUpAllPaths()
	{
		for (int i = 0; i < MapNodes.transform.childCount; i++)
		{
				for (int j = 0; j < MapNodes.transform.GetChild (i).GetComponent<Node> ().GetAllPaths ().Length; j++) 
				{
					MapNodes.transform.GetChild (i).GetComponent<Node> ().GetAllPaths ()[j].pathDirection = MovementType.TwoWay;
					MapNodes.transform.GetChild (i).GetComponent<Node> ().GetAllPaths () [j].GetComponent<LineRenderer> ().sharedMaterial = AccessibleMat;
					foreach(Transform child in MapNodes.transform.GetChild(i).transform)
					{
						if (child.tag == "Target") 
						{
						meshRend = child.GetComponent<SpriteRenderer> ();
							meshRend.sharedMaterial = AccessibleMat;
						if (FindObjectOfType<playerObj>().getStageRank(MapNodes.transform.GetChild(i).name) == 3)
							{
							child.transform.localScale = new Vector3 (child.transform.localScale.x, child.transform.localScale.y, child.transform.localScale.z*2f);
							child.GetComponent<SpriteRenderer> ().sharedMaterial = goldenMaterial;
							}
						} 					
					}
				}
		}
	}

	public void setUpStarRank(int i)
	{
		GameObject starRank = Instantiate (Resources.Load ("Other/WorldMapRank")) as GameObject;
		starRank.transform.position = new Vector3 (MapNodes.transform.GetChild (i).transform.localPosition.x, MapNodes.transform.GetChild (i).transform.localPosition.y+2, MapNodes.transform.GetChild (i).transform.localPosition.z);
		starRank.transform.SetParent (MapNodes.transform.GetChild(i).transform);
		if (SaveSystem.stageToRank[MapNodes.transform.GetChild (i).name] == 3) {
			starCount += 3;
		}
		if (SaveSystem.stageToRank[MapNodes.transform.GetChild (i).name] == 2) {
			starRank.transform.GetChild (2).GetComponent<SpriteRenderer> ().color = new Color(0.2f, 0.2f, 0.2f, 1);
			starCount += 2;
		} else if (SaveSystem.stageToRank[MapNodes.transform.GetChild (i).name] == 1) {
			starRank.transform.GetChild (0).GetComponent<SpriteRenderer> ().color = new Color(0.2f, 0.2f, 0.2f, 1);
			starRank.transform.GetChild (2).GetComponent<SpriteRenderer> ().color = new Color(0.2f, 0.2f, 0.2f, 1);
			starCount += 1;
		}
		else if (SaveSystem.stageToRank[MapNodes.transform.GetChild (i).name] == 0)
		{
			GameObject.Destroy(starRank);
		}
	}

	public void UnlockCompletedStages()
	{

		//FIRST CHECK ALL COMPLETED STAGES AND CALCULATE STARS
		for (int i = 0; i < MapNodes.transform.childCount; i++) {
			//Debug.Log (MapNodes.transform.GetChild (i).name);
			//if (FindObjectOfType<playerObj>().stageToRank.ContainsKey(MapNodes.transform.GetChild (i).name)) 
			if(SaveSystem.stageToRank.ContainsKey(MapNodes.transform.GetChild(i).name))
			{
				setUpStarRank (i);
				for (int j = 0; j < MapNodes.transform.GetChild (i).GetComponent<Node> ().GetAllPaths ().Length; j++) {
					//TODO: THIS NEEDS TO GO BACK TO AN "ORIGINAL" MOVEMENT-TYPE TO RESTRICT MAP
					MapNodes.transform.GetChild (i).GetComponent<Node> ().GetAllPaths () [j].pathDirection = MovementType.TwoWay;
					MapNodes.transform.GetChild (i).GetComponent<Node> ().GetAllPaths () [j].GetComponent<LineRenderer> ().sharedMaterial = AccessibleMat;
				}
			}
		}


		//AFTER STARS ARE CALCULATED SEE IF CONDITIONS ARE MET TO UNLOCK STAGES
		for (int i = 0; i < MapNodes.transform.childCount; i++)
		{
		 if (MapNodes.transform.GetChild (i).GetComponent<starLock> ())
				checkStarLocks (MapNodes.transform.GetChild (i).gameObject);
			
		}

	}

	public void particleAtEndPoint(GameObject node)
	{
		if(Vector3.Distance(node.transform.position, Maincam.transform.position) <= 100)
			{
				GameObject part = Instantiate (Resources.Load ("Other/EdgeParticle")) as GameObject;
				part.transform.position = node.transform.position;
				part.transform.SetParent (node.transform);
		}

	}

	public void populateMaptiles()
	{
		for (int i = 0; i < MapNodes.transform.childCount; i++) {
			GameObject nodeTile = Instantiate (Resources.Load ("Tiles/Tile3")) as GameObject;
		//	nodeTile.GetComponent<SpriteRenderer> ().sharedMaterial.color = FindObjectOfType<sceneryManager> ().wrongMat.color;
//			Debug.Log (FindObjectOfType<sceneryManager> ().wrongMat.color);
			nodeTile.transform.position = new Vector3 (MapNodes.transform.GetChild (i).transform.position.x, MapNodes.transform.GetChild (i).transform.position.y+0.1f, MapNodes.transform.GetChild (i).transform.position.z);
			nodeTile.transform.SetParent (MapNodes.transform.GetChild (i));
		}	
	}

	public void colorBlockedAndEndPoints()
	{
		for (int i = 0; i < MapNodes.transform.childCount; i++)
			
		{	foreach(Transform child in MapNodes.transform.GetChild(i).transform)
			{
				if (child.tag == "Target") 
				{
					meshRend = child.GetComponent<SpriteRenderer> ();
					if (MapNodes.transform.GetChild (i).GetComponent<Node> ().pathEndPoint () || (SaveSystem.stageToRank.ContainsKey(MapNodes.transform.GetChild (i).name) && SaveSystem.stageToRank[MapNodes.transform.GetChild (i).name] == 0 ) )
					{
//						Debug.Log (MapNodes.transform.GetChild(i).name);
						particleAtEndPoint (MapNodes.transform.GetChild(i).gameObject);
						meshRend.sharedMaterial= goldenMaterial;
						}
					else 
					{
						
						if(!SaveSystem.stageToRank.ContainsKey(MapNodes.transform.GetChild(i).name))
						//if (!FindObjectOfType<playerObj>().stageToRank.ContainsKey(MapNodes.transform.GetChild (i).name)) 
						{
							//PUT IT HERE
							child.GetComponent<SpriteRenderer> ().material.color = Color.red;
							//	nodeTile.GetComponent<SpriteRenderer> ().sharedMaterial.color = FindObjectOfType<sceneryManager> ().wrongMat.color;
							//meshRend.sharedMaterial = BlockedMat;
						}
					}	
					child.transform.localScale = new Vector3 (child.transform.localScale.x, child.transform.localScale.y, 1f);
				}
			}	
		}
	}

	public void checkStageLocks(GameObject node)
	{
		node.GetComponent<stageLock> ().showLock ();

	}


	public void checkStarLocks(GameObject node)
	{
				if (starCount < node.GetComponent<starLock>().Lock) 	
				{
					node.GetComponent<starLock>().showLock ();
					// for (int j = 0; j < node.GetComponent<Node> ().GetAllPaths ().Length; j++) 
					// {
					// 	node.GetComponent<Node> ().GetAllPaths () [j].pathDirection = MovementType.Impassable;
					// 	node.GetComponent<Node> ().GetAllPaths () [j].GetComponent<LineRenderer> ().sharedMaterial = FindObjectOfType<WorldMapManager>().BlockedMat;
					// }

				}
				else
				{
					for (int j = 0; j < node.GetComponent<Node> ().GetAllPaths ().Length; j++) 
					{
						//node.GetComponent<Node> ().GetAllPaths () [j].pathDirection = MovementType.TwoWay;
						//node.GetComponent<Node> ().GetAllPaths () [j].GetComponent<LineRenderer> ().sharedMaterial = FindObjectOfType<WorldMapManager>().AccessibleMat;
					}

				}
	}

	public void openUpFinishedPaths()
	{
		populateMaptiles ();
		UnlockCompletedStages ();
		colorBlockedAndEndPoints ();
	}

	void trackNode (Node targetNode)
	{
		curStage = targetNode.name;
		FindObjectOfType<playerObj> ().setLocation (targetNode.transform.GetSiblingIndex ());
		PlayerPrefs.SetString ("stageToLoad", curStage);
		//THIS SHOULD BE MENU LAUNCH WITH A BUTTON TO LAUNCH STAGE
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape))
			StartCoroutine (FadeToQuit ());

			
	}

	public void setPerspectiveCamera()
	{
		Maincam.orthographic = false;
		PlayerPrefs.SetInt ("Camera", 0);
		GameObject.FindGameObjectWithTag ("BG").transform.localScale = new Vector3 (60, 1, 120);
	}

	public void setOrthographicCamera()
	{
		Maincam.orthographic = true;
		PlayerPrefs.SetInt ("Camera", 1);
		GameObject.FindGameObjectWithTag ("BG").transform.localScale = new Vector3 (30, 1, 30);
	}

	public void ToggleTrail()
	{
		if (PlayerPrefs.GetInt ("TrailRenderer") == 1) {
			PlayerPrefs.SetInt ("TrailRenderer", 0);
		} else {
			PlayerPrefs.SetInt ("TrailRenderer", 1);
		}
		TrailRendererText ();
	}

	public void ToggleHitGraphic()
	{
		if (PlayerPrefs.GetInt ("HitGraphic") == 1) {
			PlayerPrefs.SetInt ("HitGraphic", 0);
		} else {
			PlayerPrefs.SetInt ("HitGraphic", 1);
		}
		HitGraphicText ();
	}

	public void ToggleMusic()
	{
		if (GameObject.FindObjectOfType<AudioScript> () != null) {
			if (PlayerPrefs.GetInt ("MusicOn") == 1) {
				PlayerPrefs.SetInt ("MusicOn", 0);
				Soundmanager_reference.Pause ();
			} else {
				PlayerPrefs.SetInt ("MusicOn", 1);
				Soundmanager_reference.UnPause ();
			}
			MusicButtonText ();
		}
	}


	public void HitGraphicText()
	{
		if (PlayerPrefs.GetInt ("HitGraphic") == 1) 
		{
			HitGraphicToggleText.text = "Turn\nHit Graphic\nOff";
		} 
		else
			HitGraphicToggleText.text = "Turn\nHit Graphic\nOn";
	}


	public void TrailRendererText()
	{
		if (PlayerPrefs.GetInt ("TrailRenderer") == 1) 
		{
			TrailButtonText.text = "Turn\nTrail Off";
		} 
		else
			TrailButtonText.text = "Turn\nTrail On";
	}

	public void MusicButtonText()
	{
		if (PlayerPrefs.GetInt ("MusicOn") == 1) 
		{
			MusicToggleText.text = "Turn\nMusic Off";
		} 
		else
			MusicToggleText.text = "Turn\nMusic On";
	}
		
	public void HapticButtonText()
	{
		if (PlayerPrefs.GetInt ("HapticOn") == 1) 
		{
			HapticText.text = "Turn\nVibration Off";
		} 
		else
			HapticText.text = "Turn\nVibration On";

	}

	public void garageButton_function()
	{
		StartCoroutine (FadeToGarage ());
	}


	public void storeButton_function()
	{
		StartCoroutine (FadeToStore ());
	}

	public void exitbutton_function()
	{
		//NEEDITHERE
		//PlayerPrefs.DeleteAll ();
		StartCoroutine(FadeToQuit());
	}

	public void ToggleHaptic()
	{
		if (PlayerPrefs.GetInt ("HapticOn") == 1)
			PlayerPrefs.SetInt ("HapticOn", 0);
		else
			PlayerPrefs.SetInt ("HapticOn", 1);
		HapticButtonText ();
	}


	IEnumerator FadeToStore()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => SceneFader.color.a == 1);
		SceneManager.LoadScene ("Store");
	}

	IEnumerator FadeToGarage()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => SceneFader.color.a == 1);
		SceneManager.LoadScene ("Garage");
	}


	IEnumerator FadeToQuit()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => SceneFader.color.a == 1);
		Application.Quit ();
	}

	IEnumerator FadeToNextStage()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => SceneFader.color.a == 1);

		//RIGHT HERE
		if (curStage[0].ToString() == "S")
			SceneManager.LoadScene ("Arsenal");
		else
			SceneManager.LoadScene (curStage);
	
	}

	public void Start_buttonfunction()
	{

		
		StartCoroutine (FadeToNextStage ());
	}

	public void logOut()
	{
		FindObjectOfType<FirebaseObject>().logOut();
		curStage = "MainScreen";
		StartCoroutine(FadeToNextStage());
		
	}
		
}
