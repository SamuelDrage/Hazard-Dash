using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public AudioClip hoverButtonSound;
    public AudioClip clickButtonSound;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.IsLoadingPanelOpen()){
            GameManager.instance.CloseLoadingPanel();
        }
        Cursor.visible = true;
    }

    public void ButtonPointerEnter(GameObject obj){
         SoundManager.instance.PlaySingle(hoverButtonSound);
        obj.GetComponent<Animation>().Play("ButtonMouseEnter");
    }

    public void ButtonPointerLeave(GameObject obj){
        obj.GetComponent<Animation>().Play("ButtonMouseLeave");
    }

    public void ButtonClick(){
        SoundManager.instance.PlaySingle(clickButtonSound);
    }

    public void QuitGame(){
        #if UNITY_EDITOR
         // Application.Quit() does not work in the editor so
         // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
         UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
