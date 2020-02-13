using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance = null;

    [Header("Player Settings")]
    public float moveSpeed;
    public float jumpForce;
    public float gravityScale;
    public float rotateSpeed;
    public float jumpMultiplyAmount;
    public float knockBackForce;
    public float knockBackTime;

    [Header ("Required Settings")]
    public GameObject playerModel;
    public Animator anim;
    public Transform cameraPivot;

    [HideInInspector]
    public bool canPlayerMove = true;
    private bool knockBacked = false;

    private CharacterController controller;
    private Vector3 moveDirection;
    private bool jumpPadJump = false;
    private bool isTeleporting = false;
    private bool teleportCooldown = false;
    private float teleportCooldownTime = 5f;

    void Awake () {
		if (instance == null){
			instance = this;
		} else if (instance != this){
			Destroy (gameObject);
		}
	}

    void Start(){
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelManager.instance.gameState != LevelManager.GameState.Playing || !canPlayerMove){
            anim.SetBool("Jumping", false);
            anim.SetFloat("Speed", 0f);
            return;
        }
        
        if(!knockBacked && !isTeleporting){
            // Store the value of the y axis to prevent gravity and jumping from being reset through normalization
            float yStore = moveDirection.y;
            // Set the move direction to the transform forward and right values times the input and move speed
            moveDirection = (transform.forward * Input.GetAxis("Vertical") * moveSpeed ) + (transform.right * Input.GetAxis("Horizontal") * moveSpeed);
            // Normalize the move direction to prevent diagonal speed gain
            moveDirection = moveDirection.normalized * moveSpeed;
            // Set the value of the y axis to the yStore variable
            moveDirection.y = yStore;

            // If the player is grounded 
            // (Needs custom grounded method using ray casting and distance checking to allow the player to walk on slopes)
            if(controller.isGrounded){
                moveDirection.y = 0f;

                // If the input keys for jumping are held down
                if(Input.GetButtonDown("Jump")){
                    moveDirection.y = jumpForce;
                }

                // If the bool jumpPadJump is set to true
                if(jumpPadJump){
                    moveDirection.y = jumpForce * jumpMultiplyAmount;
                    jumpPadJump = false;
                }
            }
        }
        
        if(!isTeleporting){
            // Apply gravity to the y direction based on the physics gravity setting, gravity scale and delta time
            moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
            // Call the move function on the controller passing in the move direction times delta time
            controller.Move(moveDirection * Time.deltaTime);

            // Move the player in different directions based on the camera direction
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
                transform.rotation = Quaternion.Euler(0f, cameraPivot.rotation.eulerAngles.y, 0);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }

            // Animations
            anim.SetBool("Jumping", !controller.isGrounded);
            anim.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Jump Pad"))
        {
            jumpPadJump = true;
        }
        if (other.gameObject.CompareTag("EndOfLevel"))
        {
            LevelManager.instance.EndLevel(LevelManager.EndLevelReasons.PlayerSuccess);
        }
        if (other.gameObject.CompareTag("DeathArea"))
        {
            LevelManager.instance.EndLevel(LevelManager.EndLevelReasons.PlayerFell);
        }
    }

    public void TeleportPlayer(Vector3 position){
        if(teleportCooldown){
            return;
        }

        teleportCooldown = true;
        StartCoroutine(TeleportPlayerRoutine(position));
    }

    private IEnumerator TeleportPlayerRoutine(Vector3 position){
        isTeleporting = true;
        transform.position = position;
        yield return new WaitForSecondsRealtime(Time.deltaTime);
        isTeleporting = false;
        yield return new WaitForSecondsRealtime(teleportCooldownTime);
        teleportCooldown = false;
    }

    public void Knockback(Vector3 direction){
        StartCoroutine(KnockBackRoutine(direction));
    }

    private IEnumerator KnockBackRoutine(Vector3 direction){
        knockBacked = true;
        moveDirection = direction * knockBackForce;
        moveDirection.y = knockBackForce;
        yield return new WaitForSecondsRealtime(knockBackTime);
        knockBacked = false;
    }
}
