using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerAnimatorManager animatorManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;

    [Header("Free Falling")]
    public Transform raycastPoint;
    public bool isGrounded;
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingSpeed =24f;
    public LayerMask groundLayer;
    public float raycastHeightOffset=1f;

    [Header("Jump")]
    public bool isJumping;
    public float jumpHeight = 3f;
    public float gravityIntensity = -15f;

    [Header("Aiming")]
    public bool isAiming;
    public bool isShooting;

    [Header("Rotation")]
    [Range(0,10)]
    public float speed=7f;
    [Range(0, 5)]
    public float rotationSpeed = 4f;
    public Vector3 targetDirection = Vector3.zero;


    public UnityAction<bool> AimingFuction; 
    public UnityAction<bool> ShootingFuction;
    public UnityAction<bool> JumpingFuction;
    private void Awake()
    {
        AddListeners();
        animatorManager = GetComponent<PlayerAnimatorManager>();    
        inputManager = GetComponent<InputManager>();    
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    private void AddListeners()
    {
        AimingFuction += HandleAiming;
        JumpingFuction += HandleJump;
        ShootingFuction += HandleShooting;
    }
    public void HandleAllMovements()
    {

        HandleFreefall();
        HandleMovement();
        HandleRotation();
    }
    private void HandleMovement()
    {
        if (isJumping || isAiming || isShooting)
        {
            return;
        }

        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * speed;
        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;

    }

    private void HandleRotation()
    {
        if (isJumping || isAiming || isShooting)
        {
            return;
        }
       

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);

        transform.rotation = playerRotation;
    }

    private void HandleFreefall()
    {
        RaycastHit hit;
        Vector3 raycastOrgin = raycastPoint.position;
        raycastOrgin.y += raycastHeightOffset;

        if (!isGrounded && !isJumping)
        {
            // Call the falling Animations here if needed
            //animatorManager.PlayTargetAnimation("Landing", true);
            //animatorManager.animator.SetBool("isLanding", true);

            inAirTimer += Time.deltaTime;
            rb.AddForce(Vector3.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingSpeed * inAirTimer);
        }
        else if (Physics.SphereCast(raycastOrgin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            // Call Landing Animation here if needed by checking !isGrounded and !isInteracting
            //animatorManager.PlayTargetAnimation("Landing", true);
            //animatorManager.animator.SetBool("isLanding", true);

            inAirTimer = 0f;
        }

        isGrounded = Physics.SphereCast(raycastOrgin, 0.2f, -Vector3.up, out hit, groundLayer);
    }
    
    private void HandleJump(bool value)
    {
        if(isGrounded)
        {
            
            isJumping = value;
            animatorManager.PlayTargetAnimation("IsJumping", value);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
            rb.velocity = playerVelocity;  
        }
    }
    private void HandleAiming(bool value)
    {
        isAiming = value;
        animatorManager.PlayTargetAnimation("IsAiming", value);
    } 
    private void HandleShooting(bool value)
    {
        isShooting = value;
        animatorManager.PlayTargetAnimation("IsShooting", value);
    }
}
