using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    CameraManager cameraManager;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();    
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraManager = FindObjectOfType<CameraManager>();

        playerLocomotion.IsJumping = animator.GetBool("isJumping");
    }

    void FixedUpdate()
    {
        playerLocomotion.HandleAllMovements();
    }
    void Update()
    {
        inputManager.HandleAllInputs();
    }
    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovements();
    }
}
