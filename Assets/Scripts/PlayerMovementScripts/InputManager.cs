using Photon.Pun.Demo.PunBasics;
using Photon.Pun.Demo.SlotRacer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    PlayerAnimatorManager playerAnimatorManager;
    PlayerLocomotion playerLocomotion;
    ShootingController shootingController;

    public Vector2 movementInput;

    public float horizontalInput;
    public float verticalInput;
    
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    float moveAmount;


    public bool Shooting;
    public bool Aiming;
    public bool Jumping;
    private void Awake()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();    
        shootingController = GetComponent<ShootingController>();
    }

    private void OnEnable()
    {
        if(playerInputActions == null)
        {
            playerInputActions = new PlayerInputActions();
            playerInputActions.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerInputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
          

            playerInputActions.PlayerMovement.Aim.started += ctx => HandleAimingInput(true);
            playerInputActions.PlayerMovement.Aim.canceled += ctx => HandleAimingInput(false);

            playerInputActions.PlayerMovement.Shooting.started += ctx => HandleShootingInput(true);
            playerInputActions.PlayerMovement.Shooting.canceled += ctx => HandleShootingInput(false);

          

            playerInputActions.PlayerActions.Jump.started += ctx => HandleJumpInput(true);
            playerInputActions.PlayerActions.Jump.canceled += ctx => HandleJumpInput(false);
        }

        playerInputActions.Enable();
    }
    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
      
       
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        playerAnimatorManager.UpdateAnimatorValues(horizontalInput, moveAmount);
    }
    private void HandleJumpInput(bool value)
    {
            Jumping = value;
             Debug.Log(Jumping);
            playerLocomotion.JumpingFuction.Invoke(value);
        
    }
    private void HandleAimingInput(bool value)
    {
        
            Aiming = value;
            playerLocomotion.AimingFuction.Invoke(value);
        
    }
    private void HandleShootingInput(bool value)
    {

        Aiming = value;
        playerLocomotion.ShootingFuction.Invoke(value);
        shootingController.ShootingFunction.Invoke();

    }
}
