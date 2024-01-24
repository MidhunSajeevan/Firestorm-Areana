using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    CameraManager cameraManager;
    public Animator animator;
    public bool isInteracting;
    private PhotonView view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();    
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraManager = FindObjectOfType<CameraManager>();

       
    }

    void FixedUpdate()
    {
        if (!view.IsMine)
            return;
        playerLocomotion.HandleAllMovements();
    }
    void Update()
    {
        if (!view.IsMine)
            return;
        inputManager.HandleAllInputs();
    }
    private void LateUpdate()
    {
        if (!view.IsMine)
            return;
        cameraManager.HandleAllCameraMovements();
       playerLocomotion.isJumping = animator.GetBool("IsJumping");
        animator.SetBool("IsGrounded", playerLocomotion.isGrounded);
   
    }
}
