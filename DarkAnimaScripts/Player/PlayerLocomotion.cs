using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{

    InputManager inputManager;
    PlayerHabilities playerHabilities;
    PhotoreceptionSystem photoreception;

    [SerializeField] Vector3 moveDirection;
    Transform cameraObject;

    Rigidbody rb;
    
    [Header("Movement Speed")]
    [Range(0, 5)] public float walkingSpeed;
    [Range(5, 15)] public float runningSpeed;
    [Range(0, 1)] public float crouchingSpeed;
    [Range(0, 40)] public float rotationSpeed;
    [Range(0,0.1f)]public float speedMultiplier;

    [Header("Fall")]
    public float inAirTime;
    public float leapingVelocity;
    public float fallingSpeed;
    public Transform sphereCheck;
    public LayerMask groundMask;
    [Range(0, 1)] public float radiusDistace;
    [Range(0, 1)]public float rayCastHeightOffSet;
    [SerializeField] private bool isGrounded;

    [Header("Colliders")]
    public CapsuleCollider normalCollider;
    public CapsuleCollider crouchCollider;

    bool crouch;

    private void Awake(){

        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        playerHabilities = GetComponent<PlayerHabilities>();
        photoreception = FindObjectOfType<PhotoreceptionSystem>();

    }

    void Start(){

        cameraObject = Camera.main.transform;

        normalCollider.enabled = true;
        crouchCollider.enabled = false;
        crouch = false;

    }

    public void HandleAllMovement(){

        HandleMovement();
        HandleRotation();
        HandleFallingAndLanding();
        

    }

    private void HandleMovement(){

        // If player is tping, movement is disabled.
        if(!playerHabilities.tping){
            // If player is concentrating, movement is disabled.
            if(inputManager.isConcentrating == 1f){
                rb.velocity = Vector3.zero;
            }
            else if(isGrounded){
                #region Player Movement Calculation.
                    moveDirection = cameraObject.forward * inputManager.verticalInput; // Moves player the direction the camera is facing.
                    moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput; // Moves player left/right based on the camera direction.
                    moveDirection.Normalize(); // Changes vector length to 1 (0 if it's too small).
                    moveDirection.y = 0;

                    if(inputManager.isRunning == 1f && inputManager.isCrouching == 0f && inputManager.isConcentrating == 0f){
                        moveDirection = moveDirection * runningSpeed; // Running.
                    }
                    if(inputManager.isRunning == 0f && inputManager.isCrouching == 0f  && inputManager.isConcentrating == 0f){
                        moveDirection = moveDirection * walkingSpeed; // Walking.
                    }
                    if(inputManager.isCrouching == 1f && inputManager.isRunning == 0f  && inputManager.isConcentrating == 0f || inputManager.isRunning == 1f){
                        moveDirection = moveDirection * crouchingSpeed; // Crouching.
                    }
                    
                    // Player colliders, changes according if player is crouching or not.
                    if(inputManager.isCrouching == 1f){
                        normalCollider.enabled = false;
                        crouchCollider.enabled = true;
                    }else{
                        normalCollider.enabled = true;
                        crouchCollider.enabled = false;
                    }

                    #region Detection of upper objects when crouching.
                    if(inputManager.isCrouching == 1f || crouch){

                        Debug.DrawRay(new Vector3(transform.position.x,transform.position.y + transform.localScale.y, transform.position.z), Vector3.up / 2f, Color.white);

                        if(Physics.Raycast(new Vector3(transform.position.x,transform.position.y + transform.localScale.y, transform.position.z), Vector3.up, 0.5f)){
                            
                            inputManager.isCrouching = 1f;
                            crouch = true;

                        }else{
                            if(crouch){
                                inputManager.isCrouching = 0f;
                                crouch = false;
                            }
                            
                        }
                    }
                    Vector3 movementVelocity;
                    #endregion
                    if(photoreception.lightValue >= 0.05f){
                        float lightClampedValue = Mathf.Clamp(photoreception.lightValue, 0f, 0.1f);   
                        movementVelocity = moveDirection / lightClampedValue * speedMultiplier;
                    }else{
                        movementVelocity = moveDirection;
                    }
                    
                    

                #endregion
                rb.velocity = movementVelocity; // Moves player according to the calculation above.
            }

        }

    }

    private void HandleRotation(){

        #region Player Rotation Calculation.
            
            Vector3 targetDirection = Vector3.zero;

            targetDirection = cameraObject.forward * inputManager.verticalInput; // Rotates the player the direction the camera is facing.
            targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput; // Rotates player left/right based on the camera direction.
            targetDirection.Normalize(); // Changes vector length to 1 (0 if it's too small).
            targetDirection.y = 0;

            if(targetDirection == Vector3.zero){
                targetDirection = transform.forward; // Keeps rotation when player stops moving instad of snapping back to origin.
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection); // Looks towards our target direction to rotate to what we are looking.
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Rotates between two points (actual player rotation -> target rotation).
        #endregion
        transform.rotation = playerRotation; // Rotates player according to the calculation above.

    }

    private void HandleFallingAndLanding(){

        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;
        rayCastOrigin.y += rayCastHeightOffSet;
        targetPosition = transform.position;

        if(!isGrounded){

            inAirTime = inAirTime + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity); // Makes player do a little jump to simulate momentum.
            rb.AddForce(Vector3.down * fallingSpeed * inAirTime); // Makes player fall when not grounded.

        }
        // Casts a sphere at the player feet in order to check if it's colliding with the ground layer.
        if(Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, 0.5f, groundMask)){

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            inAirTime = 0;
            isGrounded = true;

        }else{
            isGrounded = false;
        }

        transform.position = targetPosition;
        
    }

}
