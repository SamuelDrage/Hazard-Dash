using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float xForce;
    public float zForce;
    public float yForce;
    public float distToGround;
    public float maxSpeed;

    [Space]
    [Header("Token Settings")]
    public float spTokenMultiplyAmount;

    

    // Update is called once per frame
    void FixedUpdate()
    {
        xForce = 10.0f;
        zForce = 10.0f;
        yForce = 400.0f;
        maxSpeed = 50.0f;
        distToGround = 0.85f;

        // x Axis Movement
        float x = 0.0f;
        
        // Go Left
        if (Input.GetKey(KeyCode.A))
        {
            x = x - xForce;

            if (x < -maxSpeed)
            {
                x = -maxSpeed;
            }
        }

        // Go Right
        if (Input.GetKey(KeyCode.D))
        {
            x = x + xForce;

            if (x > maxSpeed)
            {
                x = maxSpeed;
            }
        }

        // z Axis Movement
        float z = 0.0f;
        
        // Go Forwards
        if (Input.GetKey(KeyCode.W))
        {
            z = z + zForce;

            if(z > maxSpeed)
            {
                z = maxSpeed;
            }
        }

        // Go Backwards
        if (Input.GetKey(KeyCode.S))
        {
            z = z - zForce;

            if (z < -maxSpeed)
            {
                z = -maxSpeed;
            }
        }

        // y Axis Movement
        float y = 0.0f;
        
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            y = y + yForce;
            
        }

        // Diagonal Normalizing e.g. Clamping the speed.
        Vector3 inputVector = new Vector3(xForce, 0, zForce);
        inputVector.Normalize();

        GetComponent<Rigidbody>().AddForce(x, y, z);
    }

    // Checking if the ball is currently on the ground or not.
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up") && other.gameObject.name == "Speed Token")
        {
            other.gameObject.SetActive(false);
            StartCoroutine(SpeedTokenTimer());
        }
    }

    private IEnumerator SpeedTokenTimer()
    {
        float _xForce = xForce;
        float _zForce = zForce;
        float _maxSpeed = maxSpeed;
        xForce = (float)(xForce * spTokenMultiplyAmount);
        zForce = (float)(zForce * spTokenMultiplyAmount);
        maxSpeed = (float)(maxSpeed * spTokenMultiplyAmount);
        yield return new WaitForSeconds(5);
        xForce = _xForce;
        zForce = _zForce;
        maxSpeed = _maxSpeed;
    }

}
