using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Static instance of GameManager which allows it to be accessed by any other script.
	public static GameManager instance = null;

	public GameObject fadePanel;
	public AnimationClip fadeIn;
	public AnimationClip fadeOut;
	public TextMeshProUGUI loadingLabel;
	public TextMeshProUGUI percentageLabel;
	public List<LevelData> levels;
    public Dictionary<int, LevelData> levelData;
	public int playerLevel;

    void Awake () {
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}

	public void LoadLevel(int levelNumber){
		string sceneName = levelData[levelNumber].sceneName;
		StartCoroutine(SceneChange(sceneName));
	}

	public void LoadMainMenu(){
		StartCoroutine(SceneChange("MainMenu"));
	}

	public void LoadSceneAdditive(string sceneName){
		SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
	}

	private IEnumerator SceneChange(string sceneName){
		OpenLoadingPanel();
		yield return new WaitForSeconds(0.75f);
		SceneManager.LoadScene(sceneName);
	}

	public bool IsLoadingPanelOpen(){
		return fadePanel.activeSelf;
	}

	public void OpenLoadingPanel(){
		StartCoroutine(OpenLoadingPanelRoutine());
	}

	private IEnumerator OpenLoadingPanelRoutine(){
		fadePanel.GetComponent<Animation>().clip = fadeOut;
		fadePanel.GetComponent<Animation>().Play();
		yield return new WaitForSeconds(0.25f);
		loadingLabel.gameObject.SetActive(true);
	}

	public void CloseLoadingPanel(){
		loadingLabel.gameObject.SetActive(false);
		fadePanel.GetComponent<Animation>().clip = fadeIn;
		fadePanel.GetComponent<Animation>().Play();
	}
}
