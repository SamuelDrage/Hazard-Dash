using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameResults : MonoBehaviour
{
    public GameObject levelCompletePanel;
    public GameObject levelFailedPanel;
    public AnimationClip pointerEnter;
	public AnimationClip pointerLeave;
    private int nextLevel;

    [Header("Level Complete Settings")]
    public Sprite noStar;
    public Sprite fullStar;
    public TextMeshProUGUI scoreText;
    public GameObject ratingPanel;

    [Header("Level Failed Settings")]
    public TextMeshProUGUI failReason;

    public void LevelComplete(float score, int rating, int _nextLevel){
        Cursor.visible = true;
        nextLevel = _nextLevel;
        scoreText.text = score.ToString();

        ratingPanel.transform.GetChild(0).GetComponent<Image>().sprite = noStar;
        ratingPanel.transform.GetChild(1).GetComponent<Image>().sprite = noStar;
        ratingPanel.transform.GetChild(2).GetComponent<Image>().sprite = noStar;
        if(rating >= 1){
            ratingPanel.transform.GetChild(0).GetComponent<Image>().sprite = fullStar;

            if(rating >= 2){
                ratingPanel.transform.GetChild(1).GetComponent<Image>().sprite = fullStar;

                if(rating >= 3){
                    ratingPanel.transform.GetChild(2).GetComponent<Image>().sprite = fullStar;
                }
            }
        }
        levelCompletePanel.SetActive(true);
    }

    public void LevelFailed(string reason, int _nextLevel){
        Cursor.visible = true;
        failReason.text = reason;
        nextLevel = _nextLevel;
        levelFailedPanel.SetActive(true);
    }

    public void ContinueButtonClick(){
        GameManager.instance.LoadLevel(nextLevel);
    }

    public void MainMenuButtonClick(){
        GameManager.instance.LoadMainMenu();
    }

    public void TryAgainButtonClick(){
        GameManager.instance.LoadLevel(nextLevel-1);
    }

    public void MouseEnter(GameObject obj){
        obj.GetComponent<Animation>().clip = pointerEnter;
		obj.GetComponent<Animation>().Play();
    }

    public void MouseLeave(GameObject obj){
        obj.GetComponent<Animation>().clip = pointerLeave;
		obj.GetComponent<Animation>().Play();
    }
}
