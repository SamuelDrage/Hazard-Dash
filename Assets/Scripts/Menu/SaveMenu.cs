using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    public GameObject continueLockPanel;
    public AnimationClip pointerEnter;
	public AnimationClip pointerLeave;

    // Start is called before the first frame update
    void OnEnable()
    {
        if(SaveManager.SaveFileExists()){
            continueLockPanel.SetActive(false);
        } else {
            continueLockPanel.SetActive(true);
        }
    }

    public void MouseEnter(GameObject obj){
        obj.GetComponent<Animation>().clip = pointerEnter;
		obj.GetComponent<Animation>().Play();
    }

    public void MouseLeave(GameObject obj){
        obj.GetComponent<Animation>().clip = pointerLeave;
		obj.GetComponent<Animation>().Play();
    }

    public void LoadGame(bool newGame){
        SaveManager.LoadData(newGame);
    }
}
