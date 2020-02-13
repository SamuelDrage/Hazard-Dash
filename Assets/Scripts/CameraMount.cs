using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMount : MonoBehaviour
{
    // References
	public Camera cameraComp;
	public Transform currentMount;
    public bool moveWithMount;
	private Vector3 lastPosition;

	// The speed of the transition
	public float speedFactor = 0.1f;
	//public float zoomFactor = 1.0f;

	void Start(){
		lastPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
        if(moveWithMount){
            transform.position = Vector3.Lerp(transform.position, currentMount.position, speedFactor * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, currentMount.rotation, speedFactor * Time.deltaTime);

            float velocity = Vector3.Magnitude(transform.position - lastPosition);

            lastPosition = transform.position;
        }
	}

	public void SetMount(Transform newMount){
		currentMount = newMount;
	}

    public void SetPosition(Transform newPosition){
        transform.position = newPosition.position;
        transform.rotation = newPosition.rotation;
    }
}
