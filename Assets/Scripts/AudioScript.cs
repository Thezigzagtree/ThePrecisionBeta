using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioScript : MonoBehaviour {

	public AudioSource SoundManager;
	public AudioSource SFXManager;
	public AudioSource HitSFXManager;
	public AudioSource mapSFXManager;

	[System.Serializable]
	public class SFX_set
	{
		public AudioClip[] SFXs;
	}

	private bool HitAlt = false;

	public SFX_set[] Soundeffects;
	public AudioClip[] Stagesongs;
	public int tracknum;
	public bool prevent = false;

	//Muthana Opposite the Marriot before. Fahad al salem, take a right.
	//Need passports me, sarah Salman


	public void Defeatsong()
	{
		//mapSFXManager.PlayOneShot (Defeatsong);
	}

	public void Victorysong()
	{
		//mapSFXManager.PlayOneShot (Victorysong);
	}

	public void StartPlaying()
	{
		tracknum = Random.Range (0, Stagesongs.Length); 
		SoundManager.clip = Stagesongs [tracknum];
		SoundManager.pitch = Random.Range (5,8) * 0.1f;
		SoundManager.Play ();
	}

	public void Cycletracks()
	{
		if (!SoundManager.isPlaying && prevent != true) 
		{
			SoundManager.pitch = Random.Range (5,8) * 0.1f;

			if (tracknum + 1 < Stagesongs.Length-1)
			{
				tracknum += 1;
				SoundManager.clip = Stagesongs [tracknum];
				SoundManager.Play ();
			} 

			else 
			{
				tracknum = 0;
				SoundManager.clip = Stagesongs [tracknum];
				SoundManager.Play ();
			}
		}
	}
		
	void Update()
	{
		if (PlayerPrefs.GetInt ("MusicOn") == 1) {
			Cycletracks ();
		}
	}

	public bool Toggle(bool Bool)
	{
		if (Bool == true) {
			GameObject.FindGameObjectWithTag ("HitSFXManager").GetComponent<AudioSource> ().pitch += 0.07f;
			return false;
		}
		else {
			
			return true;
		}
	}

	public void worldMapSFXPlayer(AudioClip SoundEffect)
	{
		
		mapSFXManager.PlayOneShot (SoundEffect);
	}
	public void PlayInitBegSFX()
	{
		SFXManager.pitch = Random.Range(0.4f, 2);
		SFXManager.PlayOneShot (Soundeffects [0].SFXs [4]);
	}

	public void playMenuStartSFX()
	{
		SFXManager.pitch = Random.Range(0.4f, 2);
		SFXManager.PlayOneShot (Soundeffects [0].SFXs [6]);
	}

	public void playMenuSFX()
	{
		SFXManager.pitch = Random.Range(0.4f, 2);
		SFXManager.PlayOneShot (Soundeffects [0].SFXs [7]);

		//SFXManager.pitch = p;
	}

	public void PlayBackSFX()
	{
		SFXManager.pitch = 1;
		SFXManager.PlayOneShot (Soundeffects [0].SFXs [8]);
	}

	public void PlayHitSFX()
	{
		HitAlt = Toggle (HitAlt);
//		Debug.Log (HitAlt);
		if (HitAlt)
			HitSFXManager.PlayOneShot (Soundeffects [0].SFXs [2]);
		else
			HitSFXManager.PlayOneShot (Soundeffects [0].SFXs [3]);
	}

	public void PlaySafeSFX()
	{
		
//		SFXManager.pitch = Random.Range (0.8f, 1.2f);
		SFXManager.pitch = 1;
		SFXManager.PlayOneShot (Soundeffects [0].SFXs [0]);//Random.Range(0,Soundeffects[tracknum].SFXs.Length)]);
	}

	public void PlayDamageSFX()
	{
//		SFXManager.pitch = Random.Range (0.8f, 1.2f);
		SFXManager.pitch = 1;
		SFXManager.PlayOneShot (Soundeffects [0].SFXs [1]);
	}

	public void PlayMovementSound()
	{
		SFXManager.pitch = Random.Range (0.4f, 2);
		SFXManager.PlayOneShot (Soundeffects [0].SFXs [5]);
	}
}
