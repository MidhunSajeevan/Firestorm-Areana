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


    public bool Shooting = false;
    public bool Aiming= false;
    public bool Jumping;
    public bool ReloadInput;
    private void Awake()
    {
        References();
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

            playerInputActions.PlayerActions.Reload.started += i => HandleReloadInput();
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
            playerLocomotion.JumpingFuction.Invoke(value);
        
    }
    private void HandleAimingInput(bool value)
    {
        
            Aiming = value;
            playerLocomotion.AimingFuction.Invoke(value);
        
    }
    private void HandleShootingInput(bool value)
    {

        Shooting = value;
        if(value)
            shootingController.ShootingFunction.Invoke();


        playerLocomotion.ShootingFuction.Invoke(value);
      

    }
    private void HandleReloadInput()
    {
       
        shootingController.ReloadingFunction.Invoke();

    }

    private void References()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        shootingController = GetComponent<ShootingController>();
    }
}
