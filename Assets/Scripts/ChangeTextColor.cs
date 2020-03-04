using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ChangeTextColor : MonoBehaviour {

	public GameObject MainMenuDisplay;
	public GameObject MainMenu;
	public GameObject LowerUI;
	private bool Started;

	private Vector3 DampVel;
	private Vector3 DampVel2;
	private Vector3 DampVel3;
	public float Smoothdamp = 0.5f;
	private Color Dest_color = Color.white;
	private Color[] colorset;

	public void fillcolorset()
	{
		colorset = new Color [4];
		colorset [0] = Color.cyan;
		colorset [1] = Color.green;
		colorset [2] = Color.white;
		colorset [3] = Color.yellow;
	}


	public void TouchToBegin()
	{
		GameObject.FindGameObjectWithTag ("MainScreenManager").GetComponent<MainscreenManager> ().nextbutton_function ();
	}

	void Awake()
	{
		fillcolorset ();
	}


	void Update ()
	{
		this.GetComponentInChildren<Text> ().color = Color.Lerp (this.GetComponentInChildren<Text> ().color, Dest_color, 0.15f);

	}

	void LateUpdate () 
	{
		if (this.GetComponentInChildren<Text> ().color == Dest_color) {
			Dest_color = colorset [Random.Range (0, colorset.Length)];
		}
	}
}