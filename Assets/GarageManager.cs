using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GarageManager : MonoBehaviour {

	public GameObject agentDisplay;
	public GameObject rgbController;
	public Text controller1Title;
	public GameObject rgbController2;
	public GameObject rgbController3;
	public Text controller2Title;
	public Slider redSlider;
	public Slider greenSlider;
	public Slider blueSlider;
	public Slider redSlider2;
	public Slider greenSlider2;
	public Slider blueSlider2;
	public GameObject lockedText;
	public Slider redSlider3;
	public Slider greenSlider3;
	public Slider blueSlider3;
	public Image fullColor3;

	public GameObject shapeChangeCanvas;

	public Image fullColor;
	public Image fullColor2;
	public Image SceneFader;
	public Material agentMat1;
	public Material agentMat2;
	public Animator anim;
	public Button changeShapeButton;
	public Button auraButton;
	public Button accentButton;

	private List<System.Action> toAnimate = new List<System.Action>();
	private bool rgbVisible = false;
	private bool shapeChangeVisible = false;
	private bool accentVisible = false;
	private Vector3 rgbOGposition;
	private Vector3 changeShapeButtonOGposition;
	private Vector3 auraButtonOGposition;
	private Vector3 accentButtonOGposition;
	private Vector3 rgb2OGposition;
	public Animator CanvasAnimator;

	public void resetButtonNames()
	{
		changeShapeButton.GetComponentInChildren<Text> ().text = "Change\nShape";
		auraButton.GetComponentInChildren<Text> ().text = "Aura\nColor";
		accentButton.GetComponentInChildren<Text> ().text = "Shape\nColor";
	}

	public bool checkMenu()
	{
		if (rgbVisible || shapeChangeVisible || accentVisible)
			return true;
		else
			return false;
	}

	public void saveAura()
	{
		
		PlayerPrefs.SetFloat ("redLight", redSlider3.value/255);
		PlayerPrefs.SetFloat ("greenLight", greenSlider3.value/255);
		PlayerPrefs.SetFloat ("blueLight", blueSlider3.value/255);

	}

	public void saveAndExit()
	{
		saveAura ();
		StartCoroutine (FadeToMap ());

	}


	void Start()
	{
		shapeChangeCanvas.transform.localScale = new Vector3 (0.0005f, 0.0005f, 0.0005f);
		rgbOGposition = rgbController.transform.position;
		rgb2OGposition = rgbController2.transform.position;
		changeShapeButtonOGposition = changeShapeButton.transform.position;
		auraButtonOGposition = auraButton.transform.position;
		accentButtonOGposition = accentButton.transform.position;
		redSlider3.value = agentDisplay.GetComponentInChildren<Light> ().color.r * 255;
		greenSlider3.value = agentDisplay.GetComponentInChildren<Light> ().color.g * 255;
		blueSlider3.value = agentDisplay.GetComponentInChildren<Light> ().color.b * 255;
		redSlider2.value = agentMat2.color.r * 255;
		greenSlider2.value = agentMat2.color.g * 255;
		blueSlider2.value = agentMat2.color.b * 255;

	}


	public void lockTextOn()
	{
		for (int i = 0; i < lockedText.GetComponentsInChildren<Text> ().Length; i++)
			lockedText.GetComponentsInChildren<Text>()[i].enabled = true;
		
	}

	public void lockTextOff()
	{
		for (int i = 0; i < lockedText.GetComponentsInChildren<Text> ().Length; i++)
			lockedText.GetComponentsInChildren<Text>()[i].enabled = false;
		
	}

	public void changeAccent()
	{
		accentVisible = !accentVisible;
		if (accentVisible) 
		{
			redSlider.value = agentMat1.color.r * 255;
			greenSlider.value = agentMat1.color.g * 255;
			blueSlider.value = agentMat1.color.b * 255;
			redSlider2.value = agentMat2.color.r * 255;
			greenSlider2.value = agentMat2.color.g * 255;
			blueSlider2.value = agentMat2.color.b * 255;
			accentButton.GetComponentInChildren<Text> ().text = "Return";
			CanvasAnimator.SetBool ("ShowShapeColor", true);
			//toAnimate.Add (revealAccent);
		} 

		else {
			CanvasAnimator.SetBool ("ShowShapeColor", false);
			accentButton.GetComponentInChildren<Text> ().text = "Shape\nColor";
		}


	}

	public void changeShape()
	{
		shapeChangeVisible = !shapeChangeVisible;

		if (shapeChangeVisible) {
			
			changeShapeButton.GetComponentInChildren<Text> ().text = "Return";
			CanvasAnimator.SetBool ("ChangeShape", true);

		} 
		else 
		{
			
			//toAnimate.Clear ();

			//toAnimate.Add (returnToSetup);
			changeShapeButton.GetComponentInChildren<Text> ().text = "Change\nShape";
			lockTextOff ();

			CanvasAnimator.SetBool ("ChangeShape", false);
			refreshAgent ();

		}
	}
	public void changeAura()
	{
		rgbVisible = !rgbVisible;

		if (rgbVisible) {
			CanvasAnimator.SetBool ("ShowAura", true);
			redSlider3.value = agentDisplay.GetComponentInChildren<Light> ().color.r * 255;
			greenSlider3.value = agentDisplay.GetComponentInChildren<Light> ().color.g * 255;
			blueSlider3.value = agentDisplay.GetComponentInChildren<Light> ().color.b * 255;
			auraButton.GetComponentInChildren<Text> ().text = "Return";
			//toAnimate.Add (revealAura);
		} else {
			auraButton.GetComponentInChildren<Text> ().text = "Aura\nColor";
			saveAura ();
			CanvasAnimator.SetBool ("ShowAura", false);
			//toAnimate.Clear ();

			//toAnimate.Add (returnToSetup);
		}
	}
	public void updateImageColor()
	{
		fullColor3.GetComponent<Image> ().color = new Color (redSlider3.value / 255, greenSlider3.value / 255, blueSlider3.value / 255);
		agentDisplay.GetComponentInChildren<Light> ().color = fullColor3.GetComponent<Image>().color;

	}


	public void updateShapeColor()
	{
		agentMat1.color = new Color (redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255);
		fullColor.color = new Color (redSlider.value / 255, greenSlider.value / 255, blueSlider.value / 255);
		agentMat2.color = new Color (redSlider2.value / 255, greenSlider2.value / 255, blueSlider2.value / 255);
		fullColor2.color = agentMat2.color = new Color (redSlider2.value / 255, greenSlider2.value / 255, blueSlider2.value / 255);
	}

	void Update()
	{
		
		if (rgbVisible)
			updateImageColor ();

		if (accentVisible)
			updateShapeColor ();

		if (shapeChangeVisible) {
			
		}
		for (int i = 0; i < toAnimate.Count; i++)
			toAnimate [i] ();

		if (Input.GetKeyDown (KeyCode.Escape))
			saveAndExit ();
		
	}

	IEnumerator FadeToMap()
	{
		FindObjectOfType<FirebaseObject>().saveAgentColors(agentMat1.color, agentMat2.color, new Color(PlayerPrefs.GetFloat ("redLight"), PlayerPrefs.GetFloat ("greenLight"), PlayerPrefs.GetFloat ("blueLight")));
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => SceneFader.color.a == 1);
		SceneManager.LoadScene ("WorldMap");
	}


	public void refreshAgent()
	{
		for (int i = 0; i < GameObject.FindGameObjectWithTag ("Player").transform.childCount; i++) {
			Destroy (GameObject.FindGameObjectWithTag ("Player").transform.GetChild (i).gameObject);
		}

		GameObject shape = Instantiate (Resources.Load ("AgentShapes/" + SaveSystem.GetString("currentAgentShape"))) as GameObject;
		//GameObject shape = Instantiate (Resources.Load("AgentShapes/Obelisk")) as GameObject;
		shape.transform.SetParent (GameObject.FindGameObjectWithTag("Player").transform);
		shape.transform.localPosition = new Vector3 (0, 4, 0);
		shape.GetComponent<Light> ().color = new Color (PlayerPrefs.GetFloat ("redLight"), PlayerPrefs.GetFloat ("greenLight"), PlayerPrefs.GetFloat ("blueLight"));
	}


}
