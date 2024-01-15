using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private Transform cameratransform;
    private float rotationspeed = 5f;

    private PlayerInput playerInput;
    private InputAction MoveAction;
    private InputAction JumpAction;


    private void Start()
    {
        cameratransform =Camera.main.transform;
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        MoveAction = playerInput.actions["Move"];
        JumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector2 input = MoveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameratransform.right.normalized + move.z * cameratransform.forward.normalized;
        controller.Move(move * Time.deltaTime * playerSpeed);

       
        // Changes the height position of the player..
        if (JumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        float targetAngle= cameratransform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation= Quaternion.Lerp(transform.rotation, rotation,rotationspeed * Time.deltaTime);
    }
}