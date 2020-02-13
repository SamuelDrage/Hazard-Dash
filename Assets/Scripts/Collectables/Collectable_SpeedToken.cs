using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_SpeedToken : MonoBehaviour
{
    public float speedMultiplier;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SpeedTokenTimer(other.gameObject.GetComponent<PlayerMovement>()));
        }
    }

    private IEnumerator SpeedTokenTimer(PlayerMovement playerScript)
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        
        float oldSpeed = playerScript.moveSpeed;
        playerScript.moveSpeed = playerScript.moveSpeed * speedMultiplier;
        yield return new WaitForSeconds(5);
        playerScript.moveSpeed = oldSpeed;
        Destroy(gameObject);
    }
}
