using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    public AnimationClip titleAnimation;
	public AnimationClip pointerEnter;
	public AnimationClip pointerLeave;
    public GameObject levelsPanel;
    public GameObject[] levels;
    public GameObject[] levelsLocked;

    void OnEnable()
    {
        foreach(GameObject lvl in levelsLocked){
			lvl.SetActive(true);
		}

        if (GameManager.instance.playerLevel > 1){
			for(int i = 0; i < GameManager.instance.playerLevel; i++){
				levelsLocked[i].SetActive(false);
                levels[i].GetComponent<LevelPanel>().SetupLevelPanel(GameManager.instance.levelData[i+1].score, GameManager.instance.levelData[i+1].rating);
			}
		} else {
			levelsLocked[0].SetActive(false);
            levels[0].GetComponent<LevelPanel>().SetupLevelPanel(GameManager.instance.levelData[1].score, GameManager.instance.levelData[1].rating);
		}

        levels[GameManager.instance.playerLevel-1].GetComponent<Animation>().clip = titleAnimation;
        levels[GameManager.instance.playerLevel-1].GetComponent<Animation>().Play();
		levels[GameManager.instance.playerLevel-1].transform.localScale = new Vector3(1.1f, 1.1f, 1f);
    }

    public void MouseEnter(GameObject obj){
        if(!obj.transform.Find("Locked Panel").gameObject.activeSelf && !obj.GetComponent<Animation>().IsPlaying("Title Animation")){
            obj.GetComponent<Animation>().clip = pointerEnter;
			obj.GetComponent<Animation>().Play();
        }
    }

    public void MouseLeave(GameObject obj){
        if(!obj.transform.Find("Locked Panel").gameObject.activeSelf && !obj.GetComponent<Animation>().IsPlaying("Title Animation")){
            obj.GetComponent<Animation>().clip = pointerLeave;
			obj.GetComponent<Animation>().Play();
        }
    }

    public void LevelClick(int levelNum){
        levels[GameManager.instance.playerLevel-1].transform.localScale = new Vector3(1f, 1f, 1f);
        GameManager.instance.LoadLevel(levelNum);
    }
}
