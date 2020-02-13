using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Object : MonoBehaviour
{
    public int energyToRemove;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 hitDirection = other.transform.position - transform.position;
            hitDirection = hitDirection.normalized;
            PlayerMovement.instance.Knockback(hitDirection);
            LevelManager.instance.RemoveEnergy(400);
        }
    }
}
