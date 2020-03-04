using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;

public class AdController : MonoBehaviour {


	private string storeIdAndroid = "3437536";
	private string storeIdApple = "3437537";

	private string videoAd = "video";
	private string rewardedVideo = "rewardedVideo";
	// Use this for initialization

	void Start () {
		
		Monetization.Initialize (storeIdAndroid, true);
		//Monetization.Initialize (storeIdApple, true);

	}

	// Update is called once per frame

	public void showVideoAd()
	{
		if (Monetization.IsReady (videoAd)) {

			ShowAdPlacementContent ad = null;
			ad = Monetization.GetPlacementContent (videoAd) as ShowAdPlacementContent;

			if (ad != null) {
				ad.Show ();
			}
		
		}
	}
	public void showRewardedVideo()
	{
		if (Monetization.IsReady (rewardedVideo)) 
		{

			ShowAdPlacementContent ad = null;
			ad = Monetization.GetPlacementContent (rewardedVideo) as ShowAdPlacementContent;

			if (ad != null) {
				ad.Show ();

			}

		}

	}

	public bool checkAdStatus()
	{
		return Monetization.IsReady (rewardedVideo);
	}

	public void GenerateAdNode()
	{
		
	}

	public void GenerateRandomAd()
	{
		if(Random.Range(0,100) >= 91)
			FindObjectOfType<AdController> ().showVideoAd ();
		
	}
}
