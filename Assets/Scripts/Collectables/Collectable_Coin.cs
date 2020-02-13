using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Coin : MonoBehaviour
{
    public int scoreToAdd;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LevelManager.instance.AddScore(scoreToAdd);
            Destroy(this.gameObject);
        }
    }
}
