using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public Slider gameSlider;
    public Slider musicSlider;

    public void OnEnable(){
        gameSlider.value = SoundManager.instance.efxSource.volume;
        musicSlider.value = SoundManager.instance.musicSource.volume;
    }

    public void ChangeGameVolume(){
        if(SoundManager.instance != null){
            SoundManager.instance.SetGameVolume(gameSlider.value);
        }
    }

    public void ChangeMusicVolume(){
        if(SoundManager.instance != null){
            SoundManager.instance.SetMusicVolume(musicSlider.value);
        }
    }
}
