using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    [Header("Level Settings")]
    public float currentScore;
    public int rating;
    public float distanceTraveled;
    public int levelStartTime;
    public int maxPlayerEnergy;
    public int currentPlayerEnergy;
    public int coinsToCollect;

    [Header("Rating Settings")]
    public float reqScoreZeroStar;
    public float reqScoreOneStar;
    public float reqScoreTwoStar;
    public float reqScoreThreeStar;

    [Header("Other Settings")]
    public GameObject levelManagerCanvas;
    public GameObject countDownObject;
    public TextMeshProUGUI countDownText;
    public GameState gameState = GameState.Starting;
    public enum GameState {Starting, Playing, Paused, Ended}
    public enum EndLevelReasons {PlayerEnergyRanOut, PlayerFell, PlayerQuit, PlayerSuccess}

    void Awake () {
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

    void Start(){
        StartCoroutine(LevelStartCountDown());
        StartCoroutine(LevelTick());
    }

    // Called when the level needs to end for a specific reason
    public void EndLevel(EndLevelReasons reason){
        gameState = GameState.Ended;
        switch(reason){
            case EndLevelReasons.PlayerEnergyRanOut:
                break;
            case EndLevelReasons.PlayerFell:
                break;
            case EndLevelReasons.PlayerQuit:
                break;
            case EndLevelReasons.PlayerSuccess:
                int finalRating = CalculatePlayerRating();
                break;
        }
    }

    // Calculates the players rating based on the current score and required score
    public int CalculatePlayerRating(){
        if(currentScore >= reqScoreZeroStar){
            if(currentScore >= reqScoreOneStar){
                return 1;
            } else if(currentScore >= reqScoreTwoStar){
                return 2;
            } else if(currentScore >= reqScoreThreeStar){
                return 3;
            }
            return 0;
        } else {
            return 0;
        }
    }

    // Handles starting the level
    private IEnumerator LevelStartCountDown(){
        yield return new WaitForSecondsRealtime(1f);
        countDownObject.SetActive(true);
        while(levelStartTime > 0){
            countDownText.text = levelStartTime.ToString();
            levelStartTime--;
            yield return new WaitForSecondsRealtime(1f);
        }

        countDownText.text = "GO!";
        gameState = GameState.Playing;
        yield return new WaitForSecondsRealtime(1f);
        countDownObject.SetActive(false);
    }

    // Handles the level during the game
    private IEnumerator LevelTick(){
        while(gameState == GameState.Playing || gameState == GameState.Paused)
        {
            yield return new WaitForSeconds(1f);
            currentPlayerEnergy -= 100;

            if(currentPlayerEnergy <= 0){
                EndLevel(EndLevelReasons.PlayerEnergyRanOut);
            }
        }
    }

    public void AddEnergy(int _amount)
    {
        currentPlayerEnergy = currentPlayerEnergy + _amount;
        if(currentPlayerEnergy > maxPlayerEnergy)
        {
            currentPlayerEnergy = maxPlayerEnergy;
        }
    }

    public void RemoveEnergy(int _amount)
    {
        currentPlayerEnergy = currentPlayerEnergy - _amount;
        if(currentPlayerEnergy < 0)
        {
            currentPlayerEnergy = 0;
        }
    }
}
