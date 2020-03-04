	using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class storeManager : MonoBehaviour {


	public Animator anim;
	public Image SceneFader;
	public GameObject buttonScrollView;
	public Canvas mainCanvas;
	public Canvas powerUpCanvas;
	public Canvas inGamePowerUpCanvas;
	public Canvas adCanvas;
	public Button adButton;
	public GameObject powerUpScrollView;
	public GameObject inGamePowerUpScrollView;
	public GameObject randomScrollView;
	public GameObject adScrollView;
	public Canvas faderCanvas;
	public GameObject purchaseWindow;
	public GameObject generateWindow;
	private Vector3 ogPurchasePosition;

	private Vector3 camDest;
	[HideInInspector]
	public int credits;
	[HideInInspector]
	public int silverCredits;
	private bool purchaseMoving = false;
	private bool generateMoving = false;

	private int randomItemCost = 400;

	private List <System.Action> ToAnimate = new List<System.Action>();


	public Text title;

	private string purchaseCurrency;
	private AudioSource Soundmanager_referece;

	public int getRandomItemCost()
	{
		return randomItemCost;
	}

	public void playInitBeg()
	{
		if (GameObject.FindObjectOfType<AudioScript> () != null)
		Soundmanager_referece.GetComponent<AudioScript> ().PlayInitBegSFX ();
	}


	public void playMenuStartSFX()
	{
		if (GameObject.FindObjectOfType<AudioScript> () != null)
		Soundmanager_referece.GetComponent<AudioScript> ().playMenuStartSFX ();
	}


	public void playMenuSFX()
	{
		if (GameObject.FindObjectOfType<AudioScript> () != null)
		Soundmanager_referece.GetComponent<AudioScript> ().playMenuSFX ();
	}

	public void playBacksFX()
	{
		if (GameObject.FindObjectOfType<AudioScript> () != null)
		Soundmanager_referece.GetComponent<AudioScript> ().PlayBackSFX ();
	}
		

	public void checkFirstTime()
	{

		for (int i = 0; i < GameObject.FindGameObjectWithTag ("powerUpContent").transform.childCount; i++) {
			if (FindObjectOfType<playerObj>().hasItem(GameObject.FindGameObjectWithTag ("powerUpContent").transform.GetChild (i).name))
				FindObjectOfType<playerObj>().inventoryItems[GameObject.FindGameObjectWithTag ("powerUpContent").transform.GetChild (i).name] = 0;
				//	PlayerPrefs.SetInt (GameObject.FindGameObjectWithTag ("powerUpContent").transform.GetChild (i).name, 0);	
		}

		for (int i = 0; i < GameObject.FindGameObjectWithTag ("inGamePowerUpContent").transform.childCount; i++) {
			if (!FindObjectOfType<playerObj> ().hasItem (GameObject.FindGameObjectWithTag ("inGamePowerUpContent").transform.GetChild (i).name))
				FindObjectOfType<playerObj> ().inventoryItems [GameObject.FindGameObjectWithTag ("inGamePowerUpContent").transform.GetChild (i).name] = 0;
		}
	}

	void Awake()
		{
		checkFirstTime ();
		if (GameObject.FindGameObjectWithTag ("SoundManager")) {
			Soundmanager_referece = GameObject.FindGameObjectWithTag ("SoundManager").GetComponent<AudioSource> ();
		}
		faderCanvas.enabled = false;
		ogPurchasePosition = purchaseWindow.transform.position;
		powerUpScrollView.transform.localScale = new Vector3 (0, 0, 0);
		inGamePowerUpScrollView.transform.localScale = new Vector3 (0, 0, 0);
	

		populatePowerUps ();
		populateInGamePowerUps ();
		refreshCurrentCurrencies ();

		}

	public GameObject parseInGameItemSynthesis(int i)
	{
		
		if (FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemCostType () == "Credits") {
				return Instantiate (Resources.Load ("Shop/regularInGameItem")) as GameObject;
		} else if (FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemCostType () == "silverCredits") {
				return Instantiate (Resources.Load ("Shop/silverInGameItem")) as GameObject;
			} else
				return Instantiate (Resources.Load ("Shop/regularInGameItem")) as GameObject;
		}




	public void generateRandomItem()
	{
		if (FindObjectOfType<playerObj>().credits >= randomItemCost)
		{
			
			FindObjectOfType<playerObj> ().spendCredits (randomItemCost);
			refreshCurrentCurrencies ();
			string itemName, itemDesc;

			FindObjectOfType<itemBank> ().getRandomItem (out itemName, out itemDesc);
			generateWindow.GetComponentInChildren<generateItem> ().setItemParams (itemName, itemDesc);
			FindObjectOfType<playerObj> ().addItem (itemName);
			init_GenerateScreen ();
		}

	}
		


	public GameObject parseItemSynthesis(int i)
	{
		
		if (FindObjectOfType<itemBank> ().getItemList () [i].getItemCostType () == "Credits") {
				return Instantiate (Resources.Load ("Shop/regularItem")) as GameObject;
		} else if (FindObjectOfType<itemBank> ().getItemList () [i].getItemCostType () == "silverCredits") {
				return Instantiate (Resources.Load ("Shop/silverItem")) as GameObject;
			} else
				return Instantiate (Resources.Load ("Shop/regularItem")) as GameObject;
	}
		
	public void populatePowerUps()
	{
		for (int i = 0; i < FindObjectOfType<itemBank> ().getItemList ().Count; i++) 
		{
			GameObject item = parseItemSynthesis(i);
			item.GetComponentInChildren<itemParamSet> ().refresh (FindObjectOfType<itemBank> ().getItemList () [i].getItemName (), FindObjectOfType<itemBank> ().getItemList () [i].getItemDescription (), FindObjectOfType<itemBank> ().getItemList () [i].getCostasString ());
			item.GetComponent<powerUpItem> ().setupParams ();
			item.name = FindObjectOfType<itemBank> ().getItemList () [i].getItemName ();
			item.transform.SetParent (GameObject.FindGameObjectWithTag ("powerUpContent").transform, false);


		}
	}

	public void populateInGamePowerUps ()
	{
		for (int i = 0; i < FindObjectOfType<itemBank> ().getInGameItemList ().Count; i++) 
		{
			GameObject item = parseInGameItemSynthesis(i);
			item.GetComponentInChildren<itemParamSet> ().refresh (FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemName (), FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemDescription (), FindObjectOfType<itemBank> ().getInGameItemList () [i].getCostasString ());
			item.GetComponent<powerUpItem> ().setupParams ();
			item.name = FindObjectOfType<itemBank> ().getInGameItemList () [i].getItemName ();
			item.transform.SetParent (GameObject.FindGameObjectWithTag ("inGamePowerUpContent").transform, false);


		}
		
	}

	void Update()
	{
		for (int i = 0; i < ToAnimate.Count; i++)
			ToAnimate [i] ();

		if (Input.GetKeyDown (KeyCode.Escape))
			{
				StartCoroutine (FadeToWorldMap ());
			}
	}



	public void moveToShopScreen()
	{

		powerUpCanvas.enabled = true;

	}

	public void init_ClosePurchaseScreen()
	{
		if (!purchaseMoving) {
			purchaseMoving = true;
			ToAnimate.Add (hidePurchaseScreen);
		}
	}

	public void init_CloseGenerateScreen()
	{
		if (!generateMoving) {
			generateMoving = true;
			ToAnimate.Add (hideGenerateScreen);
		}
	}


	IEnumerator FadeToWorldMap()
	{
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => SceneFader.color.a == 1);
		SceneManager.LoadScene ("WorldMap");
	}

	public void backToWorldMap()
	{
		StartCoroutine (FadeToWorldMap ());
	}

	public void init_PurchaseScreen()
	{
		
		if (!purchaseMoving) {
			
			faderCanvas.enabled = true;
			purchaseMoving = true;

			FindObjectOfType<purchaseWindowManager> ();

			ToAnimate.Add (moveInPurchaseScreen);
		}
	}

	public void init_GenerateScreen()
	{

		if (!generateMoving) {

			faderCanvas.enabled = true;
			generateMoving = true;

			FindObjectOfType<purchaseWindowManager> ();

			ToAnimate.Add (moveInGenerateScreen);
		}
		
	}
		
	public void hideGenerateScreen()
	{
		generateWindow.transform.position = Vector3.MoveTowards (generateWindow.transform.position, ogPurchasePosition, 2500 * Time.deltaTime);
		faderCanvas.GetComponentInChildren<Image>().color = Vector4.MoveTowards (faderCanvas.GetComponentInChildren<Image>().color, new Vector4 (0.1f, 0.1f, 0.1f, 0.0f), 1.5f*Time.deltaTime);
		if (generateWindow.transform.position == ogPurchasePosition && faderCanvas.GetComponentInChildren<Image> ().color.a == 0) {
			ToAnimate.Remove (hideGenerateScreen);
			faderCanvas.enabled = false;
			generateMoving = false;
		}
		
	}

	public void hidePurchaseScreen()
	{
		purchaseWindow.transform.position = Vector3.MoveTowards (purchaseWindow.transform.position, ogPurchasePosition, 2500 * Time.deltaTime);
		faderCanvas.GetComponentInChildren<Image>().color = Vector4.MoveTowards (faderCanvas.GetComponentInChildren<Image>().color, new Vector4 (0.1f, 0.1f, 0.1f, 0.0f), 1.5f*Time.deltaTime);
		if (purchaseWindow.transform.position == ogPurchasePosition && faderCanvas.GetComponentInChildren<Image> ().color.a == 0) {
			ToAnimate.Remove (hidePurchaseScreen);
			faderCanvas.enabled = false;
			purchaseMoving = false;
		}
	}

	public void moveInGenerateScreen()
	{
		generateWindow.transform.localPosition = Vector3.MoveTowards (generateWindow.transform.localPosition, new Vector3 (0, 0, 0), 2500 * Time.deltaTime);
		faderCanvas.GetComponentInChildren<Image> ().color = Vector4.MoveTowards (faderCanvas.GetComponentInChildren<Image> ().color, new Vector4 (0.1f, 0.1f, 0.1f, 0.9f), 1.5f*Time.deltaTime);

		if (generateWindow.transform.localPosition == new Vector3 (0, 0, 0) && faderCanvas.GetComponentInChildren<Image> ().color.a == 0.9f)
		{
			ToAnimate.Remove (moveInGenerateScreen);
			generateMoving = false;
		}
	}

	public void moveInPurchaseScreen()
	{
		purchaseWindow.transform.localPosition = Vector3.MoveTowards (purchaseWindow.transform.localPosition, new Vector3 (0, 0, 0), 2500 * Time.deltaTime);
		faderCanvas.GetComponentInChildren<Image> ().color = Vector4.MoveTowards (faderCanvas.GetComponentInChildren<Image> ().color, new Vector4 (0.1f, 0.1f, 0.1f, 0.9f), 1.5f*Time.deltaTime);

		if (purchaseWindow.transform.localPosition == new Vector3 (0, 0, 0) && faderCanvas.GetComponentInChildren<Image> ().color.a == 0.9f)
		{
			ToAnimate.Remove (moveInPurchaseScreen);
			purchaseMoving = false;
		}
	}

	public string parseCreditType(Vector3 col)
	{
		string parseString = col.x.ToString ("F2") + col.y.ToString ("F2") + col.z.ToString ("F2");
	//	Debug.Log (parseString);
		if (parseString == "0.940.020.02") {
			return "Credits";
		} else if (parseString == "0.200.780.78")
			return "silverCredits";
		else
			return "Invalid";
	}

	public void refreshCurrentCurrencies()
	{
		FindObjectOfType<creditCount> ().refresh ();
		FindObjectOfType<silverCreditCount> ().refresh ();
	}

	public void payForItem(string purchaseCurrency, Transform child)
	{
		if (purchaseCurrency == "Credits") {
			FindObjectOfType<playerObj> ().spendCredits (int.Parse (child.GetComponentInChildren<Text> ().text));
		} else if (purchaseCurrency == "silverCredits") {
			FindObjectOfType<playerObj> ().spendsilverCredits (int.Parse (child.GetComponentInChildren<Text> ().text));
			
		}


	}
	public void confirmPurchase(GameObject purchaseItem)
	{
		foreach (Transform child in purchaseItem.transform.GetChild(0)) 
		{
			
			if (child.tag == "itemCost") 
				{

				purchaseCurrency = parseCreditType (new Vector3 (child.GetComponent<Image> ().color.r, child.GetComponent<Image> ().color.g, child.GetComponent<Image> ().color.b));
				if (purchaseCurrency != "Invalid") 
				{
					if (purchaseCurrency == "Credits") {
						if (FindObjectOfType<playerObj> ().credits >= int.Parse (child.GetComponentInChildren<Text> ().text)) {
							playMenuStartSFX ();
							payForItem (purchaseCurrency, child);
							FindObjectOfType<playerObj> ().addItem (purchaseItem.name);
							refreshCurrentCurrencies ();
							init_ClosePurchaseScreen ();
						}
					} else if (purchaseCurrency == "silverCredits") {
						
						
						if (FindObjectOfType<playerObj> ().silverCredits >= int.Parse (child.GetComponentInChildren<Text> ().text)) {
							
							playMenuStartSFX ();
							payForItem (purchaseCurrency, child);
							FindObjectOfType<playerObj> ().addItem (purchaseItem.name);
							refreshCurrentCurrencies ();
							init_ClosePurchaseScreen ();
						}
					} else {
						playBacksFX ();
						Debug.Log ("Weird");
					}
				}
				//Debug.Log (purchaseCurrency);
			}
		}

	}

// EXPAND AND SHRINK CODE FOR SCROLLVIEWS OF CANVASES


	// POWER VIEW (POWER UPS)
	public void expandButtonView()
	{
		buttonScrollView.transform.localScale = Vector3.MoveTowards (buttonScrollView.transform.localScale, new Vector3 (1, 1, 1), 5*Time.deltaTime);
		if (buttonScrollView.transform.localScale == new Vector3 (1, 1, 1))
			ToAnimate.Remove (expandButtonView);
	}

	public void shrinkButtonView()
	{
		buttonScrollView.transform.localScale = Vector3.MoveTowards (buttonScrollView.transform.localScale, new Vector3 (0, 0, 0), 5*Time.deltaTime);
		if (buttonScrollView.transform.localScale == new Vector3 (0, 0, 0))
			ToAnimate.Remove (shrinkButtonView);

	}

	public void init_expandPowerView()
	{
		title.text = "< Power Ups >";
		if (!ToAnimate.Contains(shrinkButtonView))
			ToAnimate.Add (shrinkButtonView);
		if (!ToAnimate.Contains(expandPowerView))
			ToAnimate.Add (expandPowerView);
	}

	public void init_expandInGamePoewrUpView()
	{
		title.text = "< Abilities >";
		if (!ToAnimate.Contains(shrinkButtonView))
			ToAnimate.Add (shrinkButtonView);
		if (!ToAnimate.Contains(expandInGamePowerView))
			ToAnimate.Add (expandInGamePowerView);
		
		
	}
		
	public void init_expandRandomView()
	{
		title.text = "< Random Item >";
		if (!ToAnimate.Contains (shrinkButtonView))
			ToAnimate.Add (shrinkButtonView);
		if (!ToAnimate.Contains (expandRandomView))
			ToAnimate.Add (expandRandomView);
	}

	public void init_expandAdView()
	{
		title.text = "< Ad for Credits >";
		if (!FindObjectOfType<AdController> () || !FindObjectOfType<AdController> ().checkAdStatus () || FindObjectOfType<playerObj>().credits >= 750)
			adButton.interactable = false;
		else
			adButton.interactable = true;
		
		if (!ToAnimate.Contains (shrinkButtonView))
			ToAnimate.Add (shrinkButtonView);
		if (!ToAnimate.Contains (expandAdView))
			ToAnimate.Add (expandAdView);
		
		
	}

	public void expandAdView()
	{
		adScrollView.transform.localScale = Vector3.MoveTowards (adScrollView.transform.localScale, new Vector3 (1, 1, 1), 5*Time.deltaTime);
		if (adScrollView.transform.localScale == new Vector3 (1, 1, 1)) {
			ToAnimate.Remove (expandAdView);
		}

		
	}

	public void shrinkAdView()
	{
		adScrollView.transform.localScale = Vector3.MoveTowards (adScrollView.transform.localScale, new Vector3 (0, 0, 0), 5*Time.deltaTime);
		if (adScrollView.transform.localScale == new Vector3 (0, 0, 0))
			ToAnimate.Remove (shrinkAdView);

	}

	public void expandRandomView()
	{
		randomScrollView.transform.localScale = Vector3.MoveTowards (randomScrollView.transform.localScale, new Vector3 (1, 1, 1), 5*Time.deltaTime);
		if (randomScrollView.transform.localScale == new Vector3 (1, 1, 1)) {
			ToAnimate.Remove (expandRandomView);
		}


	}



	public void shrinkRandomView()
	{
		randomScrollView.transform.localScale = Vector3.MoveTowards (randomScrollView.transform.localScale, new Vector3 (0, 0, 0), 5*Time.deltaTime);
		if (randomScrollView.transform.localScale == new Vector3 (0, 0, 0))
			ToAnimate.Remove (shrinkRandomView);
		
	}


	public void expandInGamePowerView()
	{
		inGamePowerUpScrollView.transform.localScale = Vector3.MoveTowards (inGamePowerUpScrollView.transform.localScale, new Vector3 (1, 1, 1), 5*Time.deltaTime);
		if (inGamePowerUpScrollView.transform.localScale == new Vector3 (1, 1, 1)) {
			ToAnimate.Remove (expandInGamePowerView);
		}

	}

	public void shrinkInGamePowerUpView()
	{
		inGamePowerUpScrollView.transform.localScale = Vector3.MoveTowards (inGamePowerUpScrollView.transform.localScale, new Vector3 (0, 0, 0), 5*Time.deltaTime);
		if (inGamePowerUpScrollView.transform.localScale == new Vector3 (0, 0, 0))
			ToAnimate.Remove (shrinkInGamePowerUpView);

	}
		
	public void expandPowerView()
	{
		powerUpScrollView.transform.localScale = Vector3.MoveTowards (powerUpScrollView.transform.localScale, new Vector3 (1, 1, 1), 5*Time.deltaTime);
		if (powerUpScrollView.transform.localScale == new Vector3 (1, 1, 1)) {
			ToAnimate.Remove (expandPowerView);
		}

	}

	public void shrinkPowerView()
	{
		powerUpScrollView.transform.localScale = Vector3.MoveTowards (powerUpScrollView.transform.localScale, new Vector3 (0, 0, 0), 5*Time.deltaTime);
		if (powerUpScrollView.transform.localScale == new Vector3 (0, 0, 0))
			ToAnimate.Remove (shrinkPowerView);

	}
		
	public void init_exitRandomView()
	{
		if (ToAnimate.Contains (expandRandomView))
			ToAnimate.Remove (expandRandomView);

		if (ToAnimate.Contains (shrinkButtonView))
			ToAnimate.Remove (shrinkButtonView);

		ToAnimate.Add (expandButtonView);
		ToAnimate.Add (shrinkRandomView);
		title.text = "< Categories >";
	}

	public void init_exitAdView()
	{
		if (ToAnimate.Contains (expandAdView))
			ToAnimate.Remove (expandAdView);

		if (ToAnimate.Contains (shrinkButtonView))
			ToAnimate.Remove (shrinkButtonView);

		ToAnimate.Add (expandButtonView);
		ToAnimate.Add (shrinkAdView);
		title.text = "< Categories >";
	}


	public void init_exitPowerView()
	{
		if (ToAnimate.Contains (expandPowerView))
			ToAnimate.Remove (expandPowerView);

		if (ToAnimate.Contains (shrinkButtonView))
			ToAnimate.Remove (shrinkButtonView);

		ToAnimate.Add (expandButtonView);
		ToAnimate.Add (shrinkPowerView);
		title.text = "< Categories >";
	}		


	public void init_exitInGamePowerView()
	{
		if (ToAnimate.Contains (expandInGamePowerView))
			ToAnimate.Remove (expandInGamePowerView);

		if (ToAnimate.Contains (shrinkButtonView))
			ToAnimate.Remove (shrinkButtonView);

		ToAnimate.Add (expandButtonView);
		ToAnimate.Add (shrinkInGamePowerUpView);
		title.text = "< Categories >";
	}		

	public void initiateVision()
	{
		FindObjectOfType<AdController> ().showRewardedVideo ();
		FindObjectOfType<playerObj> ().addCredits (30);
		refreshCurrentCurrencies ();
		init_exitAdView ();
	}

}