using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformHolder : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter");
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = gameObject.transform.parent;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger Exit");
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}
