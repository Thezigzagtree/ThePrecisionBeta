using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class basic_stagemaster_functions : MonoBehaviour {



	public int score;
	private float abilityTimer = 0;
	public void setAbilityTimer(float t)
	{
		abilityTimer = t;
	}

	private float points;
	private float totalPoints = 0;

	private float bonusHP = 0;
	private float currentBonusHP;
//	private GameObject StageTargets;
	private GameObject StageManipulator;
	private GameObject Stageanimator;

	[HideInInspector]
	public Color healthyColor = new Color(0.066f, 0.5529f, 0.149f);

	//Dictionary<string, AudioClip[]> SongSoundEffect = new Dictionary<string, AudioClip[]>();

	private MonoBehaviour[] ManipulatorScripts;

	private System.Action[] HappyEndings;

	private List<Physics> hitDetectionMethod = new List<Physics> ();

	private string hitmode;
	private int speed_threshold;
	private float stagetimer;	
	private float gametimer;
	private int stage_goal;

	private Vector3 FullHPBar_OGposition;

	private Vector3 MainCameraPosition;

	//MODS
	private float hp_gain_quan;
	public Image FullHPBar;

	private GameObject[] targets;
	private GameObject[] shields;

	private Renderer rend;

	private float totalDecrease = 0;
	private float maxDecrease = 4.5f;

	[HideInInspector]
	public string current_stage;
	public Image healthbar;
	public Text scoretext;
	public Text timertext;

	public Text outcometext;

	private float maxhealth = 10.0f;
	public float getMaxHealth()
	{
		return maxhealth;
	}

	[HideInInspector]
	public float currenthealth = 10.0f;

	public float startTime;

	[HideInInspector]
	public bool victory= false;

	private int index;
	private int index2;

	public Button replaybutton;
	public Button exitbutton;

	public Image fader;
	[HideInInspector]
	public bool lost = false;
	private bool damageSound = false;

	private bool indexcorrect;

	private Dictionary<string, System.Action> hitdetection = new Dictionary<string, System.Action>();
	private Dictionary<string, System.Action> choose_target= new Dictionary<string, System.Action>();
	private List <System.Action> ToAnimate = new List<System.Action>();

	private string nextstage_name;

	//SPHERECAST
	private float SphereCastRadius = 0.035f;
	private Vector3 SphereCastOrigin;
	private Vector3 Direction;
	private LayerMask layermask = ~0;	

	private Vector3[] OG_positions;
	private Vector3[] ogScales;
	public Vector3[] getOGPositions()
	{
		return OG_positions;
	}

	private Vector3[] shieldOG_positions;
	public Vector3[] getOgScales()
	{
		return ogScales;
	}
	private bool[] OG_positionsreached;
	private bool[] shield_OGPositionreached;
	private bool[] ogScalesreached;

	[HideInInspector]
	public int comboCounter = 0;

	private int maxCombo = 0;
	public Text comboText;


	public int getTargetID(GameObject target)
	{
		for (int i = 0; i < targets.Length; i++) {
			if (targets [i] == target)
				return i;
		}

		return 0;
	}

	public float getCurrentHealth()
	{
		return currenthealth;
	}

	public void increaseCurrentHealth(float f)
	{
		if (currenthealth + f >= maxhealth)
			currenthealth = maxhealth;
		else
			currenthealth += f;
	}


	public GameObject[] getStageTargets()
	{

		return targets;

	}


	public void wrongVibration()
	{
		Vibration.Vibrate (300);
	}

	public void rightVibration()
	{
		Vibration.Vibrate (150);
	}

	public void checkComboCounter ()
	{
		if (maxCombo < comboCounter)
			maxCombo = comboCounter;

		comboText.text = "Combo\n"+maxCombo.ToString();
	}

	public void setBonusHp(float val)
	{
		bonusHP = val;
		currentBonusHP = val;
		update_bonusHP ();
	}

	void Awake()
	{
		
		//Debug.Log (Application.platform == "WindowsEditor");
		if (!GameObject.FindGameObjectWithTag("Target")) {
			GameObject Stagecore = Instantiate (Resources.Load ("Cores/" + PlayerPrefs.GetString ("stageToLoad"))) as GameObject;
		}
		StageManipulator = GameObject.FindGameObjectWithTag ("StageManipulator");
		//StageTargets = GameObject.FindGameObjectWithTag ("StageTargets");
		Stageanimator = GameObject.FindGameObjectWithTag ("StageAnimator");

		targets = GameObject.FindGameObjectsWithTag ("Target");
		shields = GameObject.FindGameObjectsWithTag ("Shield");

		Switchoff_Manipulator ();

		if (GameObject.FindObjectOfType<AudioScript> () != null)
			FindObjectOfType<AudioScript> ().GetComponent<AudioLowPassFilter> ().cutoffFrequency = 6000;

			gametimer = stagetimer;
			current_stage = PlayerPrefs.GetString ("stageToLoad");
			hitdetection.Add ("Single", single_target_detection);
			hitdetection.Add ("Double", double_target_detection);
			choose_target.Add ("Single", single_choose_random );
			choose_target.Add ("Double", double_choose_random);

			FullHPBar_OGposition = FullHPBar.GetComponent<RectTransform> ().transform.localPosition;

			ToAnimate.Add (Checkdeath);
			ToAnimate.Add (update_healthbar);


			ToAnimate.Add (SetupStage);
			SetupOGPos ();
		if (GameObject.FindGameObjectWithTag ("Shield")) {
			
			SetupShieldOGPos ();
		}
			setupOGScales ();
			resetBGshift ();
			checkComboCounter ();
		checkRevive ();
	
	}

	public void checkRevive()
	{
		if (PlayerPrefs.GetInt("revive") ==  1)
			{
			FindObjectOfType<stageStats> ().reduceStageGoal (PlayerPrefs.GetInt ("reviveScore"));
			}

	}

	public void hideBonusAtZero()
	{
		if (currentBonusHP <= 0) {
			GameObject.FindGameObjectWithTag ("bonusHP").GetComponent<Image> ().enabled = false;
			ToAnimate.Remove (hideBonusAtZero);
		}
	}

	void Update()
	{
		
		if (GameObject.FindObjectOfType<AudioScript> () != null) 
		{
			hpSoundDistortion ();
		}		
			
		if (stage_goal - score >= 0)
			scoretext.text = (stage_goal - score).ToString ();// + " / " + stage_goal.ToString();
		else
			scoretext.text = "0";
		
		if (Input.GetKeyDown (KeyCode.Escape) && victory != true)
			gameover ();
		hitdetection[hitmode]();
		if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer) {
			AddMouseDetection ();

		}

		for (int i = 0; i < ToAnimate.Count; i++) 
		{
			ToAnimate [i] ();
		}
	}

	public GameObject[] GetTargets()
	{
		return targets;
	}

	public void revealbutton (Button button)
	{
		button.GetComponent<Image> ().enabled = true;
		button.GetComponentInChildren<Text> ().enabled = true;

	}

	public void hidebutton (Button button)
	{
		button.GetComponent<Image> ().enabled = false;
		button.GetComponentInChildren<Text> ().enabled = false;
	}
		
	public void gameover()
	{
		FindObjectOfType<StageAnimator> ().checkReviveButton ();
		//FindObjectOfType<AudioScript> ().Defeatsong ();
		FindObjectOfType<RevealOptions>().AddHideOptions();
		ToAnimate.Remove (update_healthbar);
		ToAnimate.Remove (Checkdeath);
		Stageanimator.GetComponent<StageAnimator> ().HideDamage ();
		Switchoff_Manipulator ();
		//MAKE TARGETS FALL AFTER LOSING
		for (int i = 0; i < targets.Length; i++) {
			targets [i].GetComponent<MeshCollider> ().enabled = false;
			targets [i].AddComponent<Rigidbody>();
		}

		healthbar.color = Color.black;
		revealbutton (replaybutton);
		replaybutton.GetComponentInChildren<Text> ().text = "Retry";
		fader.enabled = true;
		lost = true;
		revealbutton (exitbutton);

		outcometext.text = "Defeat";
		outcometext.color = Color.red;
		Stageanimator.GetComponent<StageAnimator> ().DefeatOutcome ();
		Stageanimator.GetComponent<StageAnimator> ().LossButtons ();
	}

	public void hideui()
	{
		hidebutton (replaybutton);
		fader.enabled = false;
		hidebutton (exitbutton);

	}

	public void Switchoff_Manipulator()
	{
		ManipulatorScripts = StageManipulator.GetComponents<MonoBehaviour> ();
		for (int i = 0; i < ManipulatorScripts.Length; i++) {
			ManipulatorScripts [i].enabled = false;
		}
			
	}

	public void Switchon_Manipulator()
	{
		ManipulatorScripts = StageManipulator.GetComponents<MonoBehaviour> ();
		for (int i = 0; i < ManipulatorScripts.Length; i++) {
			ManipulatorScripts [i].enabled = true;
		}

		if (GameObject.FindGameObjectWithTag ("permaObject")) {
			if (GameObject.FindGameObjectWithTag ("permaObject").GetComponent<itemBank> ().checkPowerUpType (GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().getEquippedItem ()) == "inGame") {
				setUpInGameButton ();
				ToAnimate.Add (expandPowerUpButton);

			}
		}	

	}

	public void resetBGshift()
	{
		GameObject.FindGameObjectWithTag ("BG").GetComponent<MeshRenderer> ().sharedMaterial.SetFloat ("_Speed", 0);

	}

	public void ColorToNull()
	{	
		
		for (int i = 0; i < targets.Length; i++)
		{
			rend = targets [i].GetComponent<MeshRenderer> ();
			rend.sharedMaterial = FindObjectOfType<Stage_Pallete>().Incorrect_material;
		}
	}

	public void darkenStar ()
	{
		if (PlayerPrefs.GetInt ("revive") == 1) {
			GameObject.FindGameObjectWithTag ("Rank").transform.GetChild (2).GetComponent<Image> ().color = new Color32 (75, 75, 75, 100);
			GameObject.FindGameObjectWithTag ("Rank").transform.GetChild (3).GetComponent<Image> ().color = new Color32 (75, 75, 75, 100);

		}
		else if (currenthealth <= (maxhealth / 2) && currenthealth >= (maxhealth/3))
			GameObject.FindGameObjectWithTag ("Rank").transform.GetChild (3).GetComponent<Image> ().color = new Color32 (75, 75, 75, 100);
		else if (currenthealth <= (maxhealth / 3)) 
		{
			GameObject.FindGameObjectWithTag ("Rank").transform.GetChild (2).GetComponent<Image> ().color = new Color32 (75, 75, 75, 100);
			GameObject.FindGameObjectWithTag ("Rank").transform.GetChild (3).GetComponent<Image> ().color = new Color32 (75, 75, 75, 100);
		}
		
	}

	public int comp_score()
	{
		if (PlayerPrefs.GetInt ("revive") == 1) {
			return 1;
		}
		else if (currenthealth >= (maxhealth / 2)) 
		{
				return 3;

		} 
		else if (currenthealth >= (maxhealth / 4) && currenthealth < maxhealth / 2) {
				return 2;
			} 
		else {
				return 1;
			}
	}
		
	public void stage_complete()
	{
		if (FindObjectOfType<playerObj>().playerHasStage(current_stage))
		{
			int prev = FindObjectOfType<playerObj>().getStageRank(current_stage);
			int newScore = comp_score();

			if (newScore > prev)
				FindObjectOfType<playerObj> ().uploadStage (current_stage, comp_score ());


		}
			

		else {
			FindObjectOfType<playerObj> ().uploadStage (current_stage, comp_score ());

		}

		FindObjectOfType<StageReport> ().writeReport ((Time.timeSinceLevelLoad - startTime), (currenthealth / maxhealth), maxCombo, totalPoints);
		ColorToNull ();
		ToAnimate.Remove (update_healthbar);
		ToAnimate.Remove (Checkdeath);
		ToAnimate.Add (HappyEnding);
		Switchoff_Manipulator ();
		victory = true;
		//PLAYSONG HERE
		//FindObjectOfType<AudioScript> ().Victorysong ();

		FindObjectOfType<textSizeOsci> ().addWobble ();
		darkenStar ();
		outcometext.text = "Victory!";
		Stageanimator.GetComponent<StageAnimator> ().VictoryOutcome ();
		Stageanimator.GetComponent<StageAnimator> ().WinButtons ();
		Stageanimator.GetComponent<StageAnimator>().RevealStars();
		fader.enabled = true;
		revealbutton (exitbutton);
		revealbutton (replaybutton);

	}

	public void hpSoundDistortion()
	{
		if (currenthealth <= maxhealth / 4)
			FindObjectOfType<AudioScript> ().GetComponent<AudioLowPassFilter> ().cutoffFrequency = 2000;
		else if (currenthealth <= maxhealth / 2)
			FindObjectOfType<AudioScript> ().GetComponent<AudioLowPassFilter> ().cutoffFrequency = 4000;
	}

	public void shiftBGwithHP()
	{
		float bgWave = GameObject.FindGameObjectWithTag ("BG").GetComponent<MeshRenderer> ().sharedMaterial.GetFloat ("_Speed") + Time.fixedDeltaTime;
		GameObject.FindGameObjectWithTag ("BG").GetComponent<MeshRenderer> ().sharedMaterial.SetFloat ("_Speed", bgWave);

	}

	public void Decrease_HP()
	{
		comboCounter = 0;
		if (!damageSound) {
			damageSound = true;
			if (GameObject.FindObjectOfType<AudioScript> () != null)
				FindObjectOfType<AudioScript> ().PlayDamageSFX ();
		}
		if (currentBonusHP <= 0) {
			
			currenthealth -= Time.fixedDeltaTime;
			totalDecrease += Time.fixedDeltaTime;
			healthbar.color = Color.yellow;
			Stageanimator.GetComponent<StageAnimator> ().ShowDamage ();
			shiftBGwithHP ();
		} else {
			currentBonusHP -= Time.fixedDeltaTime;
			totalDecrease += Time.fixedDeltaTime;
			Stageanimator.GetComponent<StageAnimator> ().ShowDamage ();
			update_bonusHP ();
			
		}
	}

	public float return_healthbar()
	{
		return (currenthealth / maxhealth);
	}	

	public void update_bonusHP()
	{
		float bonusRatio = currentBonusHP / maxhealth;
		GameObject.FindGameObjectWithTag ("bonusHP").GetComponent<RectTransform> ().localScale = new Vector3 (bonusRatio, 1, 1);
	}

	public void update_healthbar()
	{
		float hpratio = currenthealth / maxhealth;
		healthbar.rectTransform.localScale = new Vector3 (hpratio, 1, 1);
	}

	public void replay()
	{
		PlayerPrefs.SetInt ("revive", 0);
		PlayerPrefs.SetInt("reviveScore", 0);

		if (GameObject.FindObjectOfType<AudioScript> () != null)
			GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch = 0.7f;
		Stageanimator.GetComponent<StageAnimator> ().Replay ("Arsenal");
	}
		
	public void exit()
	{
		if (GameObject.FindObjectOfType<AudioScript> () != null)
			GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch = 0.7f;
		
		Stageanimator.GetComponent<StageAnimator> ().Exit ();
	}
		

	public void Firstchoice()
	{
		choose_target [hitmode]();
	}



	public void CheckOGPos()
	{
		for (int i = 0; i < OG_positionsreached.Length; i++) 
		{
			if (OG_positionsreached [i] == false)
				return;
			
		}

		if (GameObject.FindGameObjectWithTag ("Shield")) 
		{
			
			for (int i = 0; i < shield_OGPositionreached.Length; i++) {
				if (shield_OGPositionreached [i] == false)
					return;
			}
		}

		finishSetup ();
	}

	public bool checkOGScales()
	{
		for (int i = 0; i < ogScales.Length; i++)
		{
			if (ogScalesreached [i] == false)
				return false;
		}

		return true;


	}


	public void modifyTrailRenderer()
	{

		foreach (GameObject target in targets) 
		{
			if (target.tag == "Correct") {

				target.GetComponent<TrailRenderer> ().startColor = Color.yellow;
				target.GetComponent<TrailRenderer> ().widthMultiplier = 0.5f;
				target.GetComponent<TrailRenderer> ().time = 3f;
			} else {
				target.GetComponent<TrailRenderer> ().startColor = new Color (FindObjectOfType<Stage_Pallete> ().Incorrect_material.color.r, FindObjectOfType<Stage_Pallete> ().Incorrect_material.color.g, FindObjectOfType<Stage_Pallete> ().Incorrect_material.color.b, 0.3f);
				target.GetComponent<TrailRenderer> ().widthMultiplier = 0.75f;
			}
		}
	}

	public void setupTrailRenderer(GameObject target)
	{
		TrailRenderer trail = target.transform.GetComponent<TrailRenderer> ();
		trail.material = new Material (Shader.Find ("Sprites/Default"));

		AnimationCurve trailCurve = new AnimationCurve();
		trailCurve.AddKey (0.0f, 0.1f);
		trailCurve.AddKey (0.2f, 0.02f);
		trailCurve.AddKey (0.4f, 0.1f);
		trailCurve.AddKey (0.6f, 0.02f);
		trailCurve.AddKey (0.8f, 0.1f);
		trailCurve.AddKey (1.0f, 0.00f);
		trail.widthCurve = trailCurve;
		trail.widthMultiplier = 1.5f;
		trail.time = 0.75f;
		trail.startColor = new Color (FindObjectOfType<Stage_Pallete> ().Incorrect_material.color.r, FindObjectOfType<Stage_Pallete> ().Incorrect_material.color.g, FindObjectOfType<Stage_Pallete> ().Incorrect_material.color.b, 0.5f) ;
		trail.endColor = new Color (FindObjectOfType<Stage_Pallete> ().Correct_material.color.r, FindObjectOfType<Stage_Pallete> ().Correct_material.color.g, FindObjectOfType<Stage_Pallete> ().Correct_material.color.b, 0.65f);
		//trail.endColor = new Color (GameObject.FindGameObjectWithTag("BG").GetComponent<MeshRenderer>().material.color.r, GameObject.FindGameObjectWithTag("BG").GetComponent<MeshRenderer>().material.color.g, GameObject.FindGameObjectWithTag("BG").GetComponent<MeshRenderer>().material.color.b, 0.15f);

	}

	public void attachTrailRenderer()
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			targets [i].AddComponent<TrailRenderer> ();
			setupTrailRenderer (targets [i]);
		}
	}


	public void shrinkShields()
	{
		for (int i = 0; i < shields.Length; i++) {
			shields [i].transform.localScale = new Vector3 (0, 0, 0);
		}
	}

	public void ScatterTargets()
	{
		
		for (int i = 0; i < targets.Length; i++) 
		{
			int J = Random.Range (0, 3);
			if (J == 0) {
				if (Random.Range(0,100) > 50)
					targets [i].transform.position = new Vector3 (targets [i].transform.position.x + 10, targets [i].transform.position.y, targets [i].transform.position.z);
				else
					targets [i].transform.position = new Vector3 (targets [i].transform.position.x - 10, targets [i].transform.position.y, targets [i].transform.position.z);
			}
			if (J == 1) {
				if (Random.Range(0,100) > 50)
					targets [i].transform.position = new Vector3 (targets [i].transform.position.x, targets [i].transform.position.y + 10, targets [i].transform.position.z);
				else
					targets [i].transform.position = new Vector3 (targets [i].transform.position.x, targets [i].transform.position.y - 10, targets [i].transform.position.z);
			}
			if (J == 2) {
				targets [i].transform.position = new Vector3 (targets [i].transform.position.x, targets [i].transform.position.y, targets [i].transform.position.z - 10);
			}
		}


		if (GameObject.FindGameObjectWithTag("Shield"))
			{
		for (int i = 0; i < shields.Length; i++) 
		{
			int J = Random.Range (0, 3);
			if (J == 0) {
				if (Random.Range(0,100) > 50)
					shields [i].transform.position = new Vector3 (shields [i].transform.position.x + 10, shields [i].transform.position.y, shields [i].transform.position.z);
				else
					shields [i].transform.position = new Vector3 (shields [i].transform.position.x - 10, shields [i].transform.position.y, shields [i].transform.position.z);
			}
			if (J == 1) {
				if (Random.Range(0,100) > 50)
					shields [i].transform.position = new Vector3 (shields [i].transform.position.x, shields [i].transform.position.y + 10, shields [i].transform.position.z);
				else
					shields [i].transform.position = new Vector3 (shields [i].transform.position.x, shields [i].transform.position.y - 10, shields [i].transform.position.z);
			}
			if (J == 2) {
				shields [i].transform.position = new Vector3 (shields [i].transform.position.x, shields [i].transform.position.y, shields [i].transform.position.z - 10);
			}
		}
			}
	}
		
	public void finishSetup()
	{
		ToAnimate.Remove (SetupStage);
		Stageanimator.GetComponent<StageAnimator> ().FinishSetUp ();
	}

	public void SetupStage()
	{
		for (int i = 0; i < OG_positions.Length; i++) {
			targets [i].transform.position = Vector3.MoveTowards (targets [i].transform.position, OG_positions [i], 5f*Time.deltaTime);
		}

		if (GameObject.FindGameObjectWithTag("Shield"))
			{
		for (int i = 0; i < shields.Length; i++) {
			shields [i].transform.position = Vector3.Slerp (shields [i].transform.position, shieldOG_positions [i], Time.deltaTime);
			shields [i].transform.position = Vector3.MoveTowards (shields [i].transform.position, shieldOG_positions [i], 2.5f* Time.deltaTime);
		}
			}

		for (int i = 0; i < OG_positions.Length; i++) 
		{
			if (targets [i].transform.position == OG_positions [i])
				OG_positionsreached [i] = true;
		}

		if (GameObject.FindGameObjectWithTag ("Shield")) {
			for (int i = 0; i < shields.Length; i++) {
				if (shields [i].transform.position == shieldOG_positions [i])
					shield_OGPositionreached [i] = true;
			}
		}
		CheckOGPos ();

	}

	public void SetupOGPos_Bools()
	{
		OG_positionsreached = new bool[targets.Length];

		for (int i = 0; i < OG_positionsreached.Length; i++) {
			OG_positionsreached [i] = false;
		}

	}

	public void setupOGscale_Bools()
	{
		ogScalesreached = new bool[targets.Length];
		for (int i = 0; i < ogScalesreached.Length; i++) {
			ogScalesreached [i] = false;
		}
		
	}

	public void setupShieldOGPos_Bools()
	{
		shield_OGPositionreached = new bool[shields.Length];
		for (int i = 0; i < shield_OGPositionreached.Length; i++) {
			shield_OGPositionreached[i] = false;
		}
	}

	public void SetupOGPos()
	{	
		OG_positions = new Vector3[targets.Length];
		SetupOGPos_Bools ();
		for (int i = 0; i < targets.Length; i++) 
		{
			OG_positions [i] = targets [i].transform.position;
		}
			
	}

	public void SetupShieldOGPos()
	{
		shieldOG_positions = new Vector3[shields.Length];
		setupShieldOGPos_Bools ();
		for (int i = 0; i < shields.Length; i++) {
			shieldOG_positions[i] = shields [i].transform.position;
		}
	}

	public void setupOGScales()
	{
		ogScales = new Vector3[targets.Length];
		setupOGscale_Bools ();
		for (int i = 0; i < targets.Length; i++) {
			ogScales[i] = targets [i].transform.localScale;
		}
		
	}

	public void shieldOGScales()
	{
		
	}

	// Use this for initialization
	void Start () 
	{
		if (PlayerPrefs.GetInt("TrailRenderer") == 1)

			attachTrailRenderer ();
		
		hitmode = GameObject.FindGameObjectWithTag("StageCore").GetComponent<stageStats>().GetHitMode(PlayerPrefs.GetString("stageToLoad"));
		speed_threshold = GameObject.FindGameObjectWithTag("StageCore").GetComponent<stageStats>().GetSpeedThreshold(PlayerPrefs.GetString("stageToLoad"));
		stagetimer = GameObject.FindGameObjectWithTag("StageCore").GetComponent<stageStats>().GetStageTimer(PlayerPrefs.GetString("stageToLoad"));	
		stage_goal = GameObject.FindGameObjectWithTag("StageCore").GetComponent<stageStats>().GetStageGoal(PlayerPrefs.GetString("stageToLoad"));
		hp_gain_quan = GameObject.FindGameObjectWithTag("StageCore").GetComponent<stageStats> ().GetStageHpGainQuan (PlayerPrefs.GetString("stageToLoad"));
		SphereCastOrigin = GameObject.FindGameObjectWithTag ("MainCamera").transform.position;
		GameObject.FindObjectOfType<Stage_Pallete> ().trueRandomPallete();
		lost = false;
		hideui ();
		ScatterTargets ();

		if (GameObject.FindGameObjectWithTag ("permaObject")) {
			if (GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().isItemEquipped ()) {
				if (GameObject.FindGameObjectWithTag ("permaObject").GetComponent<itemBank> ().checkPowerUpType (GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().getEquippedItem ()) == "preGame") {
					GameObject.FindGameObjectWithTag ("permaObject").GetComponent<itemBank> ().findItemEffect (GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().getEquippedItem ());
				} 
			}
		}
			
	}

	public bool doubleDistanceCheck(int index, int index2)
	{
		if (Vector3.Distance (targets [index].transform.position, targets [index2].transform.position) > 0.5f)
			return true;
		else
			return false;
	}

	public void double_choose_random()
	{
		indexcorrect = false;
		while (indexcorrect == false) 
		{
			index = Random.Range (0, targets.Length);
			index2 = Random.Range (0, targets.Length);

			if (index != index2) {
				if (doubleDistanceCheck (index, index2))	
					indexcorrect = true;
				Debug.Log (Vector3.Distance (targets [index].transform.position, targets [index2].transform.position));
			}
		}


		for (int i = 0; i < targets.Length; i++) 
		{
			rend = targets [i].GetComponent<MeshRenderer> ();
			if (i == index || i == index2) 
			{
				targets [i].tag = "Correct";
				rend.sharedMaterial = FindObjectOfType<Stage_Pallete> ().Correct_material;
				//rend.sharedMaterial = Stagedecorator.GetComponent<Stage_Pallete>().Correct_material;
			} 
			else 
			{
				targets [i].tag = "Incorrect";
				rend.sharedMaterial = FindObjectOfType<Stage_Pallete>().Incorrect_material;
				//rend.sharedMaterial = Stagedecorator.GetComponent<Stage_Pallete>().Incorrect_material;
			}
		}
	}
		

	public void single_choose_random()
	{
		//Choose a Random Target
		index = Random.Range(0,targets.Length);


		for (int i = 0; i < targets.Length; i++) 
		{
			rend = targets [i].GetComponent<MeshRenderer> ();
			if (i == index) 
			{
				targets [i].tag = "Correct";

				rend.sharedMaterial = FindObjectOfType<Stage_Pallete> ().Correct_material;//Correct_color;
				//rend.sharedMaterial = Stagedecorator.GetComponent<Stage_Pallete>().Correct_material;
			} 
			else 
			{
				targets [i].tag = "Incorrect";
				rend.sharedMaterial = FindObjectOfType<Stage_Pallete> ().Incorrect_material;
				//rend.sharedMaterial = Stagedecorator.GetComponent<Stage_Pallete>().Incorrect_material;
			}
				
		}


		if(PlayerPrefs.GetInt("TrailRenderer") == 1)
			modifyTrailRenderer ();

	}

	public void double_target_detection()
	{
		if (Input.touches.Length > 1 && lost != true && victory != true) 
		{
			Touch t = Input.GetTouch(0);
			Touch t2 = Input.GetTouch(1);
			if (t.phase == TouchPhase.Began || t2.phase == TouchPhase.Began) 
			{
				
				var ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				var ray2 = Camera.main.ScreenPointToRay (Input.GetTouch (1).position);

				RaycastHit hit;
				RaycastHit hit2;

				if (EventSystem.current.IsPointerOverGameObject (Input.GetTouch(0).fingerId) || (EventSystem.current.IsPointerOverGameObject (Input.GetTouch(1).fingerId)))
					return;

				//if (Physics.Raycast (ray, out hit, 100) && Physics.Raycast (ray2, out hit2, 100))
				//{
				if ((Physics.SphereCast (SphereCastOrigin, SphereCastRadius, ray.direction, out hit, 100f, layermask)) && (Physics.SphereCast (SphereCastOrigin, SphereCastRadius, ray2.direction, out hit2, 100f, layermask)))
				{
					{
						if (hit.collider.tag == "Correct" && hit2.collider.tag == "Correct" && hit.collider.gameObject != hit2.collider.gameObject) {
							score += 1;
							if (score == 1) {
								startTime = Time.timeSinceLevelLoad;
								//WRITE START TIME;
							}

							comboCounter += 1;

							checkComboCounter ();
							totalDecrease = 0;
							if (FindObjectOfType<RevealOptions> ().Optionsrevealed)
								FindObjectOfType<RevealOptions> ().AddHideOptions ();
							damageSound = false;
							if (PlayerPrefs.GetInt ("HapticOn") == 1)
								rightVibration ();

							if (gametimer <= 0) 
							{
								Stageanimator.GetComponent<StageAnimator> ().ShowDoubleHit ();
								if (GameObject.FindObjectOfType<AudioScript> () != null) {
									GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch = 0.7f;
									FindObjectOfType<AudioScript> ().PlaySafeSFX ();
								}		
							} else 
							{
								if (GameObject.FindObjectOfType<AudioScript> () != null) {
									FindObjectOfType<AudioScript> ().PlayHitSFX ();

								}
							}
								
							Stageanimator.GetComponent<StageAnimator> ().HideDamage ();
							FullHPBar.GetComponent<RectTransform> ().transform.localPosition = FullHPBar_OGposition;
							healthbar.color = healthyColor;
							if (currenthealth < maxhealth / 2) {
								currenthealth += hp_gain_quan;
								Stageanimator.GetComponent<StageAnimator> ().hpGainGlow ();
							} else
								Stageanimator.GetComponent<StageAnimator> ().Correcthit_glow ();
							
							if (score == stage_goal) {
								stage_complete ();
							} else {
								set_timer ();

								choose_target [hitmode] ();
							}
						} 

							else if (hit.collider.tag == "item") {
						}

					else if (score > 0)
					{
						if (gametimer > 0) 
							{
							gametimer = 0;
							
							wrongVibration ();
						
							}
						else
						gametimer = 0;
							
						}
					}
				}
			}
		}
	}

	public void set_timer()
	{
		if (score < speed_threshold)
			gametimer = stagetimer - score * 0.01f;
		else
			gametimer = stagetimer - speed_threshold*0.01f;
	}
		

	public void updateScore()
	{
		if (points + Time.deltaTime < totalPoints) {
			points += Time.deltaTime * 5;
		} else
			points = totalPoints;
		timertext.text = points.ToString("####"); 
	}

	public void pointCalculation(RaycastHit hit, Vector3 position)
	{
		float multiplier = checkHitDistance (hit, position);
		points = totalPoints;
		totalPoints += ((1 * (multiplier) + (100 / (Time.timeSinceLevelLoad - startTime))) * comboCounter + score)/10;

	}

	public void AddMouseDetection()
	{
		if (Input.GetMouseButtonDown (0) && !lost && !victory) 
		{

			if (EventSystem.current.IsPointerOverGameObject ())
				return;
			
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;


			//TODO
			//if (Physics.Raycast (ray, out hit, 100)) 

			if (Physics.SphereCast (SphereCastOrigin, SphereCastRadius, ray.direction, out hit, 100f, layermask))
			{
				Debug.Log(hit.collider.tag);
				if (hit.collider.tag == "Correct") 
				{
					score += 1;
					pointCalculation (hit, Input.mousePosition);
					comboCounter += 1;
					//ToAnimate.Add (updateScore);
					//timertext.text = totalPoints.ToString ("F1");
					if (score == 1) 
					{
						startTime = Time.timeSinceLevelLoad;
						//WRITE START TIME;
					}


					checkComboCounter ();

					if (PlayerPrefs.GetInt ("HitGraphic") == 1)
						
						{
							GameObject.FindGameObjectWithTag ("HitMarker").GetComponent<HitMarker> ().relocate (hit.transform.position);
							GameObject.FindGameObjectWithTag ("HitMarker").GetComponent<HitMarker> ().FadeIn ();
						}
					totalDecrease = 0;
					if (FindObjectOfType<RevealOptions> ().Optionsrevealed)
						FindObjectOfType<RevealOptions> ().AddHideOptions ();
					damageSound = false;


					if (PlayerPrefs.GetInt ("HapticOn") == 1)
						rightVibration ();
					if (gametimer <= 0) 
					{
						Stageanimator.GetComponent<StageAnimator> ().ShowCorrectHit (Input.mousePosition);
						if (GameObject.FindObjectOfType<AudioScript> () != null) 
						{
							GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch = 0.7f;
							FindObjectOfType<AudioScript> ().PlaySafeSFX ();
						}		
					} else 
					{
						if (GameObject.FindObjectOfType<AudioScript> () != null) 
						{
							FindObjectOfType<AudioScript> ().PlayHitSFX ();
							GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch += 0.07f;
						}
					}

					FindObjectOfType<StageGoalShake> ().Startshaking (); 
					Stageanimator.GetComponent<StageAnimator>().HideDamage();
					FullHPBar.GetComponent<RectTransform> ().transform.localPosition = FullHPBar_OGposition;
					healthbar.color = healthyColor;


					if (currenthealth < maxhealth / 2) {
						currenthealth += hp_gain_quan;
						Stageanimator.GetComponent<StageAnimator> ().hpGainGlow ();
					}
					else
						Stageanimator.GetComponent<StageAnimator> ().Correcthit_glow ();
					set_timer ();

					if (score >= stage_goal) {
						stage_complete ();
					}
					else
					choose_target [hitmode] ();
				} 

				else if (hit.collider.tag == "item") {
				}

				else if (score > 0)
				{
					if (gametimer > 0) 
					{
						
						gametimer = 0;
						wrongVibration ();
					}
					else
						gametimer = 0;
				}
			}
		}
	}

	//checkHitDistance (hit.distance, hit.collider.gameObject, Input.mousePosition,  hit.point);
	public float checkHitDistance(RaycastHit playerHit, Vector3 clickPos)
	{
		
		//if (Physics.Raycast (ray, out hit, 100)) 
		var ray = Camera.main.ScreenPointToRay (clickPos);
		RaycastHit rayCastHit;
		RaycastHit sphereCastHit;
		if (Physics.Raycast (ray, out rayCastHit, playerHit.distance)) {
			//Debug.Log (Vector3.Distance (playerHit.point, ray.GetPoint (playerHit.distance)));
			return 10;
		
		} else if (Physics.SphereCast (SphereCastOrigin, SphereCastRadius, ray.direction, out sphereCastHit, 100f, layermask)) {
			return 7.5f;

			//Debug.Log (Vector3.Distance (playerHit.point, ray.GetPoint (playerHit.distance)));
			//REGULAR EFFECT
		} else
			return 5;	

		
	//	Debug.Log(Vector3.Distance(ray.GetPoint(dist)

		//RAYCAST TO THE SAME AREA
		//FIND DISTANCE BETWEEN RAYCAST HIT and SPHERECAST CORRECT HIT
	}

	public void snipeShot()
	{
		score += 1;


		if (score == 1) 
		{
			startTime = Time.timeSinceLevelLoad;
			//WRITE START TIME;
		}

		comboCounter += 1;

		checkComboCounter ();

		totalDecrease = 0;

		if (FindObjectOfType<RevealOptions> ().Optionsrevealed)
			FindObjectOfType<RevealOptions> ().AddHideOptions ();

		damageSound = false;


		if (PlayerPrefs.GetInt ("HapticOn") == 1) {
			rightVibration ();

		}

		if (gametimer <= 0) {
			Stageanimator.GetComponent<StageAnimator> ().ShowCorrectHit (Input.GetTouch (0).position);
			if (GameObject.FindObjectOfType<AudioScript> () != null) 
			{
				GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch = 0.7f;
				FindObjectOfType<AudioScript> ().PlaySafeSFX ();

		}
		}
	}

	public void correctHitFunction()
	{
		score += 1;
		comboCounter += 1;
		//ToAnimate.Add (updateScore);
		//timertext.text = totalPoints.ToString ("F1");
		if (score == 1) 
		{
			startTime = Time.timeSinceLevelLoad;
			//WRITE START TIME;
		}


		checkComboCounter ();

		totalDecrease = 0;
		if (FindObjectOfType<RevealOptions> ().Optionsrevealed)
			FindObjectOfType<RevealOptions> ().AddHideOptions ();
		damageSound = false;


		if (PlayerPrefs.GetInt ("HapticOn") == 1)
			rightVibration ();


		if (gametimer <= 0) 
		{
			if (GameObject.FindObjectOfType<AudioScript> () != null) {
				GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch = 0.7f;
				FindObjectOfType<AudioScript> ().PlaySafeSFX ();
			} 
		}
		else {
			if (GameObject.FindObjectOfType<AudioScript> () != null) {
				FindObjectOfType<AudioScript> ().PlayHitSFX ();
				GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch += 0.07f;
			}
		}
		FindObjectOfType<StageGoalShake> ().Startshaking (); 
		Stageanimator.GetComponent<StageAnimator>().HideDamage();
		FullHPBar.GetComponent<RectTransform> ().transform.localPosition = FullHPBar_OGposition;
		healthbar.color = healthyColor;


		if (currenthealth < maxhealth / 2) {
			currenthealth += hp_gain_quan;
			Stageanimator.GetComponent<StageAnimator> ().hpGainGlow ();
		}
		else
			Stageanimator.GetComponent<StageAnimator> ().Correcthit_glow ();
		set_timer ();

		if (score >= stage_goal) {
			stage_complete ();
		}
		else
			choose_target [hitmode] ();



	}

	public void single_target_detection()
	{
		if (Input.touches.Length > 0 && lost != true && victory != true) 
		{
			Touch t = Input.GetTouch(0);
			if (t.phase == TouchPhase.Began) 
			{

				if (EventSystem.current.IsPointerOverGameObject (t.fingerId))
					return;
				
				var ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);
				RaycastHit hit;

				//if (Physics.Raycast (ray, out hit, 100)) 
				//{
				if (Physics.SphereCast (SphereCastOrigin, SphereCastRadius, ray.direction, out hit, 100f, layermask))
				{
					//checkHitDistance (hit.distance, hit.collider.gameObject, Input.GetTouch(0).position, hit.point);
					if (hit.collider.tag == "Correct") {
						pointCalculation (hit, Input.GetTouch (0).position);
						if (PlayerPrefs.GetInt ("HitGraphic") == 1) {
							GameObject.FindGameObjectWithTag ("HitMarker").GetComponent<HitMarker> ().relocate (hit.transform.position);
							GameObject.FindGameObjectWithTag ("HitMarker").GetComponent<HitMarker> ().FadeIn ();
						}

						correctHitFunction ();
					}
					//	Debug.Log(hit.
						//FIND OVERLAP HERE

					else if (hit.collider.tag == "Item") {
					}

					else if (score > 0) 
					{
						if (gametimer > 0) 
						{
							gametimer = 0;
							wrongVibration ();

						}
						else
							gametimer = 0;
							
						}
				}
			}
		}

	}
	public void HappyEnding()
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			targets [i].transform.position = Vector3.Slerp (targets [i].transform.position, new Vector3 (0, 0, 0),Time.deltaTime);
		}

	}

	public void timer_update()
	{
			gametimer -= Time.fixedDeltaTime;
	}
		
	public void Checkdeath()
	{
		if (gametimer <= 0) 
		{
			if (currenthealth <= 0) 
			{
				gameover ();


			} else if (score > 0 && totalDecrease < maxDecrease) {
				
				Decrease_HP ();
				Healthdecreaseanimation ();


			} else if (totalDecrease > 2) {
				Stageanimator.GetComponent<StageAnimator> ().HideDamage ();
			}
		} 
		else
			Timercountdown ();
	}

	public void Timercountdown()
	{
		if (score > 0 && victory != true)
			timer_update ();
	}
		
	public void Healthdecreaseanimation()
	{
		if (gametimer<0 && lost != true)
		{
			FullHPBar.GetComponent<RectTransform> ().transform.localPosition = new Vector3 (FullHPBar_OGposition.x + Random.Range (-2, 2), FullHPBar_OGposition.y + Random.Range (-2, 2), FullHPBar_OGposition.z + Random.Range (-2, 2));
		}
	}

// SETTING FUNCTIONS
	public void setSphereCastRadius(float f)
	{
		SphereCastRadius = f;
	}

	public void setHPGainQuan( float f)
	{
		hp_gain_quan = GameObject.FindGameObjectWithTag("StageCore").GetComponent<stageStats> ().GetStageHpGainQuan (PlayerPrefs.GetString("stageToLoad")) + f;
	}

	public void setSpeedThreshold (int s)
	{
		speed_threshold = GameObject.FindGameObjectWithTag ("StageCore").GetComponent<stageStats> ().GetSpeedThreshold (PlayerPrefs.GetString ("stageToLoad")) - s;
	}

	public void setStageTimer (float f)
	{
		stagetimer = GameObject.FindGameObjectWithTag ("StageCore").GetComponent<stageStats> ().GetStageTimer (PlayerPrefs.GetString ("stageToLoad")) + f;
	}

	public void setStageGoal (int g)
	{
		stage_goal = GameObject.FindGameObjectWithTag("StageCore").GetComponent<stageStats>().GetStageGoal(PlayerPrefs.GetString("stageToLoad")) - g;
	}

	public void setTotalDecrease(float f)
	{
		totalDecrease = f;
	}


	public void setUpInGameButton()
	{
		GameObject.FindGameObjectWithTag ("effectButton").GetComponentInChildren<Text> ().text = GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().getEquippedItem ();
		GameObject.FindGameObjectWithTag ("effectButton").GetComponentInChildren<Text> ().text.Insert (GameObject.FindGameObjectWithTag ("effectButton").GetComponentInChildren<Text> ().text.Length - 3, "\n");


	}

	public void InGameEffectButton()
	{

		GameObject.FindGameObjectWithTag ("permaObject").GetComponent<itemBank> ().findItemEffect (GameObject.FindGameObjectWithTag ("permaObject").GetComponent<equippedItems> ().getEquippedItem ());
	}

	public void revertScales()
	{
		for (int i = 0; i < targets.Length; i++) {
			targets [i].transform.localScale = Vector3.MoveTowards (targets[i].transform.localScale, ogScales [i], 10*Time.deltaTime);
		}

		if (checkOGScales()) {
			ToAnimate.Remove (revertScales);
		}
	}

	public void shrinkWave()
	{
		for (int i = 0; i < targets.Length; i++) 
		{
			
			if (targets [i].tag == "Correct")
				targets [i].transform.localScale = ogScales [i];
			else {
				Debug.Log (targets.Length);
				targets [i].transform.localScale = Vector3.Lerp (targets [i].transform.localScale, new Vector3 (ogScales [i].x * 0.75f, ogScales [i].y * 0.75f, ogScales [i].z * 0.75f), 12.5f * Time.deltaTime);
			}
		}

		abilityTimer -= Time.fixedDeltaTime;
		if (abilityTimer <= 0) {
			ToAnimate.Remove (shrinkWave);
			ToAnimate.Add (revertScales);
		}

	}

	public void init_shrinkWave(float t)
	{
		abilityTimer = t;
		ToAnimate.Add (shrinkWave);
	}

	public void init_shrinkPowerUpButton()
	{
		GameObject.FindGameObjectWithTag ("effectButton").GetComponent<Button> ().interactable = false;
		ToAnimate.Add (shrinkPowerUpButton);

		
	}

	public void expandPowerUpButton()
	{
		GameObject.FindGameObjectWithTag ("effectButton").transform.localScale = Vector3.MoveTowards (GameObject.FindGameObjectWithTag ("effectButton").transform.localScale, new Vector3 (1, 1, 1), 10*Time.deltaTime);
		if (GameObject.FindGameObjectWithTag ("effectButton").transform.localScale == new Vector3(1,1,1))
		{
			ToAnimate.Remove (expandPowerUpButton);
		}
	}

	public void shrinkPowerUpButton()
	{
		GameObject.FindGameObjectWithTag ("effectButton").transform.localScale = Vector3.MoveTowards (GameObject.FindGameObjectWithTag ("effectButton").transform.localScale, new Vector3 (0, 0, 0), 10*Time.deltaTime);
		if (GameObject.FindGameObjectWithTag ("effectButton").transform.localScale == new Vector3(0,0,0))
			{
			ToAnimate.Remove (shrinkPowerUpButton);
			}
	}

	public void playMenuStartSFX()
	{
		GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>().GetComponent<AudioScript> ().playMenuStartSFX ();
	}

	public void reviveFunction()
	{
		FindObjectOfType<AdController> ().showRewardedVideo ();
		PlayerPrefs.SetInt ("revive", 1);
		PlayerPrefs.SetInt("reviveScore", score);
		if (GameObject.FindObjectOfType<AudioScript> () != null)
			GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch = 0.7f;
		Stageanimator.GetComponent<StageAnimator> ().Replay ("Arsenal");

	}

}
