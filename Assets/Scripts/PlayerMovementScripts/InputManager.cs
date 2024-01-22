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

    public Vector2 movementInput;

    public float horizontalInput;
    public float verticalInput;
    
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    float moveAmount;
    public bool jumpInput;

    private void Awake()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();    
    }

    private void OnEnable()
    {
        if(playerInputActions == null)
        {
            playerInputActions = new PlayerInputActions();
            playerInputActions.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerInputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerInputActions.PlayerMovement.Jumping.performed += i => jumpInput = true;
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
        playerAnimatorManager.UpdateAnimatorValues(0f,moveAmount);
    }

}
