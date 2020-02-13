using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public GameObject speakingObject;
    public TextMeshProUGUI speakingText;
    public GameObject continueText;
	private bool canPlayerEndSpeak = false;
    private bool endPlayerSpeaking = false;

    void Update(){
        if(Input.GetKeyDown(KeyCode.Return)){
           EndPlayerSpeak();
        }
    }

    // Sentence: What you want to say, Textcolor: Color, SpeakDelay: Delay before speaking Endtime: How long does it display for, WriteDelay: Time per character being written
	public void CallPlayerSpeak(string sentence, Color textColor, int speakDelay, int endTime, float writeDelay){
		endPlayerSpeaking = false;
		canPlayerEndSpeak = false;
		StopAllCoroutines();
        StartCoroutine(PlayerSpeak(sentence, textColor, speakDelay, endTime, writeDelay));
	}

    // Call to end player speaking if the passed speakDelay was equal to 0
    public void EndPlayerSpeak(){
		if(canPlayerEndSpeak) {
			endPlayerSpeaking = true;
			if(!PlayerMovement.instance.canPlayerMove){
				PlayerMovement.instance.canPlayerMove = true;
			}
		}
    }

	// Handles player speaking, not to be called directly
	private IEnumerator PlayerSpeak(string sentence, Color textColor, int speakDelay, int endTime, float writeDelay){
		yield return new WaitForSeconds(speakDelay);
		speakingText.color = textColor;
        continueText.SetActive(false);
		speakingObject.gameObject.SetActive(true);
		StartCoroutine(TypeSentence(sentence, endTime, writeDelay));
	}

    // Closes our speaking functions
	private IEnumerator EndPlayerSpeak(string sentence, int endTime, float writeDelay){
		canPlayerEndSpeak = true;
        if(endTime == 0){
            while(!endPlayerSpeaking){
                yield return new WaitForSeconds(0.5f);
            }
        } else {
		    yield return new WaitForSeconds(endTime);
        }

		continueText.SetActive(false);
		int textLength = speakingText.text.Length;
		for(int i = 0; i < textLength; i++){
			speakingText.text = speakingText.text.Remove(speakingText.text.Length-1);
			yield return new WaitForSeconds(writeDelay);
		}

		speakingObject.gameObject.SetActive(false);
	}
	
	// Handles writing our sentence
	private IEnumerator TypeSentence(string sentence, int endTime, float writeDelay){
		speakingText.text = "";
		foreach(char letter in sentence.ToCharArray()){
			speakingText.text += letter;
			yield return new WaitForSeconds(writeDelay);
		}

        continueText.SetActive(true);
		StartCoroutine(EndPlayerSpeak(sentence, endTime, writeDelay));
	}
}
