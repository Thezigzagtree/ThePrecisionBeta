using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ContrastIncreaseScript : MonoBehaviour {

		public Material screenEffect;
		void OnRenderImage (RenderTexture src, RenderTexture dest) 
		{
			Graphics.Blit (src, dest, screenEffect);

		}
	}
