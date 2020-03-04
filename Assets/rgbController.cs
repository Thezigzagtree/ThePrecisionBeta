using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class rgbController : MonoBehaviour {

	public Slider redSlider;
	public Slider greenSlider;
	public Slider blueSlider;
	public Image fullColor;
	// Use this for initialization
	void Start () {

		redSlider.value = 20;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
