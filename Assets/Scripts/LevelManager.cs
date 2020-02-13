using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    [Header("Level Settings")]
    public int levelNumber;
    public int lives;
    public float score;
    public int rating;
    public int maxEnergy;
    public int energy;
    public float distanceTraveled;
    public int levelStartTime;

    [Header("Rating Settings")]
    public float reqScoreZeroStar;
    public float reqScoreOneStar;
    public float reqScoreTwoStar;
    public float reqScoreThreeStar;

    [Header("Camera Settings")]
    public Transform cameraMountStart1;
    public Transform cameraMountEnd1;
    public Transform cameraMountStart2;
    public Transform cameraMountEnd2;
    public Transform cameraMountStart3;
    public Transform cameraMountEnd3;

    [Header("Other Settings")]
    public GameObject playerUI;
    public TextMeshProUGUI playerScoreUI;
    public TextMeshProUGUI playerEnergyUI;
    public TextMeshProUGUI playerLivesUI;
    public Slider playerEnergySlider;
    public GameObject gameResultsCanvas;
    public GameObject countDownObject;
    public TextMeshProUGUI countDownText;
    public GameState gameState = GameState.Starting;
    public enum GameState {Starting, Playing, Paused, Ended}
    public enum EndLevelReasons {PlayerEnergyRanOut, PlayerFell, PlayerLivesRanOut, PlayerSuccess}

    void Awake () {
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
	}

    void Start(){
        StartCoroutine(LevelStartCountDown());
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.H)){
            EndLevel(EndLevelReasons.PlayerSuccess);
        }
    }

    // Called when the level needs to end for a specific reason
    public void EndLevel(EndLevelReasons reason){
        gameState = GameState.Ended;
        playerUI.SetActive(false);
        switch(reason){
            case EndLevelReasons.PlayerEnergyRanOut:
                gameResultsCanvas.GetComponent<GameResults>().LevelFailed("Your energy ran out!", levelNumber+1);
                break;
            case EndLevelReasons.PlayerFell:
                gameResultsCanvas.GetComponent<GameResults>().LevelFailed("You fell to your death!", levelNumber+1);
                break;
            case EndLevelReasons.PlayerSuccess:
                int finalRating = CalculatePlayerRating();

                if(GameManager.instance.playerLevel <= levelNumber)
                {
                    GameManager.instance.playerLevel = levelNumber+1;
                }

                SaveLevelDetails();
                gameResultsCanvas.GetComponent<GameResults>().LevelComplete(score, finalRating, levelNumber+1);
                break;
        }
    }

    private void SaveLevelDetails(){
        if(score > GameManager.instance.levelData[levelNumber].score){
            GameManager.instance.levelData[levelNumber].score = score;
            GameManager.instance.levelData[levelNumber].rating = CalculatePlayerRating();
            SaveManager.SaveData(GameManager.instance.playerLevel, GameManager.instance.levelData);
        }
    }

    // Calculates the players rating based on the current score and required score
    public int CalculatePlayerRating(){
        if(score >= reqScoreThreeStar){
            return 3;
        } else if(score >= reqScoreTwoStar){
            return 2;
        } else if(score >= reqScoreOneStar){
            return 1;
        } else {
            return 0;
        }
    }

    // Handles starting the level
    private IEnumerator LevelStartCountDown(){
        Cursor.visible = false;
        if(GameManager.instance != null){
            GameManager.instance.CloseLoadingPanel();
        }
        yield return new WaitForSecondsRealtime(0.2f);

        countDownObject.SetActive(true);
        while(levelStartTime > 0){
            if(levelStartTime == 3){
                Camera.main.GetComponent<CameraMount>().SetPosition(cameraMountStart1);
                Camera.main.GetComponent<CameraMount>().SetMount(cameraMountEnd1);
            } else if(levelStartTime == 2){
                Camera.main.GetComponent<CameraMount>().SetPosition(cameraMountStart2);
                Camera.main.GetComponent<CameraMount>().SetMount(cameraMountEnd2);
            } else {
                Camera.main.GetComponent<CameraMount>().SetPosition(cameraMountStart3);
                Camera.main.GetComponent<CameraMount>().SetMount(cameraMountEnd3);
            }


            countDownText.text = "0" + levelStartTime.ToString();
            countDownText.GetComponent<Animation>().Play();
            levelStartTime--;
            yield return new WaitForSecondsRealtime(1.5f);
        }

        Camera.main.GetComponent<CameraMount>().moveWithMount = false;
        Camera.main.GetComponent<PlayerCamera>().enableCamera = true;
        countDownText.text = "GO!";
        countDownText.GetComponent<Animation>().Play();
        gameState = GameState.Playing;
        StartCoroutine(LevelTick());

        yield return new WaitForSecondsRealtime(1f);
        countDownObject.SetActive(false);
        playerUI.SetActive(true);
        if(levelNumber == 1){
            playerUI.GetComponent<PlayerUI>().CallPlayerSpeak("Welcome to the tutorial level for Hazard Dash. Use the WASD keys to move, SPACE to jump and the mouse to rotate the camera", Color.white, 0, 0, 0.03f);
            PlayerMovement.instance.canPlayerMove = false;
        }
        UpdatePlayerUI();
    }

    // Handles the level during the game
    private IEnumerator LevelTick(){
        while(gameState == GameState.Playing){
            yield return new WaitForSeconds(1f);
            energy -= 50;
            UpdatePlayerUI();

            if(energy <= 0){
                energy = 0;
                EndLevel(EndLevelReasons.PlayerEnergyRanOut);
                break;
            }
        }
    }

    public void AddEnergy(int _amount)
    {
        energy += _amount;
        if(energy > maxEnergy)
        {
            energy = maxEnergy;
        }
        UpdatePlayerUI();
    }

    public void RemoveEnergy(int _amount)
    {
        energy -= _amount;
        if(energy <= 0)
        {
            energy = 0;
            EndLevel(EndLevelReasons.PlayerEnergyRanOut);
        }

        lives--;
        if(lives <= 0){
            EndLevel(EndLevelReasons.PlayerLivesRanOut);
        }
        UpdatePlayerUI();
    }

    public void AddScore(int _amount){
        score += _amount;
        UpdatePlayerUI();
    }

    public void UpdatePlayerUI(){
        //playerScoreUI.text = score.ToString();
        //playerEnergyUI.text = energy.ToString();
        playerEnergySlider.maxValue = maxEnergy;
        playerEnergySlider.value = energy;
    }
}
