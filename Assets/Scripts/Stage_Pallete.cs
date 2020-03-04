using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stage_Pallete : MonoBehaviour {


	//INCASE OF SHADER CHANGE ADD THIS LINE in OUTLINEDEFAULT
	//_ASEOutlineWidth = abs(cos(_Time[1]*5))*_ASEOutlineWidth;

	[System.Serializable]
	public class Pallete
	{
		public Vector3[] colors = new Vector3[3];
	}
		
	private Pallete[] Palletes;

	public Material Correct_material;
	public Material Incorrect_material;
	public Material BG_material;
	public Material BG_BufferColor;
	public Material Accent_color;
	public Material trailMat;
	public Material shieldMat;

	public Texture[] Backgrounds;

	public GameObject Backgroundblock;

	public Image Correcthit;
	//[HideInInspector]
	public Texture[] wrongMatPat;
	//public Color[,,] Colorpalletes = new Color[2,2,2];

	public void ColorShields()
	{
		if (GameObject.FindGameObjectWithTag ("Shield"))
			for (int i = 0; i < GameObject.FindGameObjectsWithTag ("Shield").Length; i++) {
				GameObject.FindGameObjectsWithTag ("Shield") [i].GetComponent<MeshRenderer> ().material = shieldMat;
			}
		
	}

	void Awake()
	{
		SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
	}


	public Color generateCorrectColor()
	{
		return new Color (Random.Range (4f, 15) * 0.05f, Random.Range (4f, 15) * 0.05f, Random.Range (4f, 15) * 0.05f);
	}

	public void mutateIncorrect()
	{
		Incorrect_material.SetTextureScale ("_MainTex", new Vector2 (Random.Range (0.1f, 1), Random.Range (0.1f, 1)));
		if(Incorrect_material.GetTextureScale("_MainTex").x > 0.5f && Incorrect_material.GetTextureScale("_MainTex").y > 0.5f)
			Incorrect_material.SetTextureOffset ("_MainTex", new Vector2 (Random.Range(0, (1 - Incorrect_material.GetTextureScale ("_MainTex").x)), Random.Range(0, (1 - Incorrect_material.GetTextureScale ("_MainTex").y))));
		else
			Incorrect_material.SetTextureOffset ("_MainTex", new Vector2 (Random.Range(0, Incorrect_material.GetTextureScale ("_MainTex").x), Random.Range(0, Incorrect_material.GetTextureScale ("_MainTex").y)));
		
	}

	public void trueRandomPallete()
	{
		BG_material.SetTexture ("_MainTex", Backgrounds [Random.Range (0, Backgrounds.Length)]);
		Incorrect_material.color = generateCorrectColor();
		Incorrect_material.SetTexture("_MainTex", wrongMatPat[Random.Range(0, wrongMatPat.Length)]);
		mutateIncorrect ();
		Correct_material.color = generateCorrectColor ();
		Correct_material.SetColor ("Outline Color", Incorrect_material.color);
		BG_BufferColor.SetColor ("_EmissionColor", generateCorrectColor ());




		if (Random.Range (0, 100) < 50) 
		{
			Backgroundblock.transform.Rotate (new Vector3 (0, 0, 180));
		}

	}



	private void SwipeDetector_OnSwipe(SwipeData data)
	{
		if (data.Direction == SwipeDirection.Right && FindObjectOfType<basic_stagemaster_functions>().score==0)
		{
		//	FindObjectOfType<Stage_Pallete> ().Shufflecolors();

		}
		if (data.Direction == SwipeDirection.Down  && FindObjectOfType<basic_stagemaster_functions>().score==0) {

		}
		if (data.Direction == SwipeDirection.Left && FindObjectOfType<basic_stagemaster_functions>().score==0)
		{
		//	FindObjectOfType<Stage_Pallete> ().Shufflecolors();


		}
		if (data.Direction == SwipeDirection.Up  && FindObjectOfType<basic_stagemaster_functions>().score==0)
		{

		}
	}
		
	public void Shufflecolors()
	{
		trueRandomPallete ();
	}


	void Start()
	{
		ColorShields ();
		trueRandomPallete ();
	//	RandomPallete ();

	}

}
