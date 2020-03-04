using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WavyBGScript : MonoBehaviour {

	public Material BGmat;

	void OnRenderImage (RenderTexture src, RenderTexture dest) 
	{
		Graphics.Blit (src, dest, BGmat);

	}

}
