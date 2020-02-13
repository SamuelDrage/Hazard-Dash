using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Teleporter : MonoBehaviour
{   
    public Transform targetSpawner;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Do we get here?");
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement.instance.TeleportPlayer(targetSpawner.transform.position);
        }
    }
}
