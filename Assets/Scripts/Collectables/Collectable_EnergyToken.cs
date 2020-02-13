using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_EnergyToken : MonoBehaviour
{
    public int energyToAdd;
    public int scoreToAdd;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LevelManager.instance.AddEnergy(energyToAdd);
            LevelManager.instance.AddScore(scoreToAdd);
            Destroy(this.gameObject);
        }
    }
}
