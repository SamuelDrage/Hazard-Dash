using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	//Static instance of SoundManager which allows it to be accessed by any other script.
	public static SoundManager instance = null;
	
	// Audio Sources
	public AudioSource efxSource;
	public AudioSource musicSource;

	// Background Music clips
	public List<AudioClip> menuMusicList = new List<AudioClip>();

	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;
	public bool automaticBackgroundMusic = true;
	private int lastMusicIndex = -1;

	void Awake () {
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad(gameObject);

		if(menuMusicList.Count != 0){
			if(automaticBackgroundMusic){
				RandMusic(0);
			}

			// Load player prefered volume from file
			if(PlayerPrefs.HasKey("MusicVolume")){
				musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
			}
		}

		// Load player prefered volume from file
		if(PlayerPrefs.HasKey("GameVolume")){
			efxSource.volume = PlayerPrefs.GetFloat("GameVolume");
		}
	}

	void Update(){
		if(menuMusicList.Count != 0 && automaticBackgroundMusic){
			if(!musicSource.isPlaying){
				RandMusic(1);
			}
		}
	}

	public void SaveVolumeSettings(){
		PlayerPrefs.Save();
	}

	public void SetGameVolume(float value){
		efxSource.volume = value;
		PlayerPrefs.SetFloat("GameVolume", efxSource.volume);
		SaveVolumeSettings();
	}

	public void SetMusicVolume(float value){
		musicSource.volume = value;
		PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);
		SaveVolumeSettings();
	}

	// Play random music from the musicList
	public void RandMusic(float delay){
		int randIndex;
		if(menuMusicList.Count > 1){
			randIndex = Random.Range(0, menuMusicList.Count);
		} else {
			randIndex = 0;
		}

		if(randIndex == lastMusicIndex){ RandMusic(delay); return;}
		
		lastMusicIndex = randIndex;
		musicSource.clip = menuMusicList[randIndex];
		musicSource.PlayDelayed(delay);
	}

	public void PauseUnpauseMusic(){
		if(musicSource.isPlaying){
			musicSource.Pause();
		} else {
			musicSource.UnPause();
		}
	}

	public void RestartMusic(){
		musicSource.time = 0;
	}

	public void SetMusic(AudioClip clip){
		musicSource.clip = clip;
		musicSource.Play();
	}

	public void PlaySingle(AudioClip clip){
		efxSource.clip = clip;
		efxSource.PlayOneShot(efxSource.clip);
	}

	public void RandomizeSfx(params AudioClip[] clips){
		int randomIndex;
		if(clips.Length > 1) {
			randomIndex = Random.Range(0, clips.Length); 
		} else { 
			randomIndex = 0; 
		}
		
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		efxSource.pitch = randomPitch;
		efxSource.clip = clips[randomIndex];
		efxSource.PlayOneShot(efxSource.clip);
	}

	void OnApplicationQuit(){
		SaveVolumeSettings();
		Debug.Log("Application ending after " + Time.time + " seconds");
	}
}
