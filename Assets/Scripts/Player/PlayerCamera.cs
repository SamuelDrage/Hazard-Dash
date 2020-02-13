using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool useOffsetValues;
    public float rotateSpeed;
    public Transform pivot;

    public float maxViewAngle;
    public float minViewAngle;
    public bool invertY;

    public bool enableCamera;

    void Start(){
        if(!useOffsetValues){
            offset = target.position - transform.position;
        }

        pivot.transform.position = target.transform.position;
        pivot.transform.parent = null;
    }

    void LateUpdate(){
        if(enableCamera){
            pivot.transform.position = target.transform.position;

            // Get the X position of the mouse and rotate the pivot
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            pivot.Rotate(0, horizontal, 0);

            // Get the Y position of the mouse and rotate the pivot
            float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
            if(invertY){
                pivot.Rotate(-vertical, 0, 0);
            } else {
                pivot.Rotate(vertical, 0, 0);
            }

            float desiredYAngle = pivot.eulerAngles.y;
            float desiredXangle = pivot.eulerAngles.x;

            // Prevents the camera from pivoting too far above the player
            if(pivot.rotation.eulerAngles.x > maxViewAngle && pivot.eulerAngles.x < 180f){
                pivot.rotation = Quaternion.Euler(maxViewAngle, desiredYAngle, 0);
            }

            // Prevents the camera from pivoting too far below the player
            if(pivot.rotation.eulerAngles.x > 180f && pivot.eulerAngles.x < 360 + minViewAngle){
                pivot.rotation = Quaternion.Euler(360f + minViewAngle, desiredYAngle, 0);
            }    

            Quaternion rotation = Quaternion.Euler(desiredXangle, desiredYAngle, 0);
            transform.position = target.position - (rotation * offset);

            if(transform.position.y < target.position.y){
                transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
            }

            transform.LookAt(target);
        }
    }
}
