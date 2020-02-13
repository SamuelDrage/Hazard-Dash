using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public string sentence;
    public bool triggered;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !triggered){
            TutorialSpeak();
        }
    }

    public void TutorialSpeak(){
        PlayerMovement.instance.canPlayerMove = false;
        LevelManager.instance.playerUI.GetComponent<PlayerUI>().CallPlayerSpeak(sentence, Color.white, 0, 0, 0.03f);
        triggered = true;
    }
}
