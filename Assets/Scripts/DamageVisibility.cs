using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisibility : MonoBehaviour {

	public Animator anim;
//	private List<System.Action> ToAnimate = new List<System.Action>();

	void Start ()
	{
		//anim.StopPlayback ();
		//rend = this.gameObject.GetComponent<SpriteRenderer> ();
	}

	public void HideDamage()
	{
		anim.SetBool ("Damage", false);
//		if(ToAnimate.Contains(FadeToDamage))
//			ToAnimate.Remove(FadeToDamage);
		
//		ToAnimate.Add (FadeToAlpha);
		//	rend.enabled = false;
	}

	public void ShowDamage()
	{

		anim.SetBool ("Damage", true);
		//yield return new WaitUntil (() => Blackbox.color.a == 1);
		//SceneManager.LoadScene("Tutorial");


	//	if(ToAnimate.Contains(FadeToAlpha))
	//		ToAnimate.Remove(FadeToAlpha);
	//	ToAnimate.Add (FadeToDamage);
		//rend.enabled = true;
	}


}
