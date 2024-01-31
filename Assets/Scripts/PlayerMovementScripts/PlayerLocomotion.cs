
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    PlayerAnimatorManager animatorManager;
    ShootingController shootingController;
    PlayerControlerManager playercontrolerManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;
    PhotonView photonView;

    [Header("Player Health")]
    const float maxHealth = 150f;
    public float currentHealth;
    public GameObject PlayerHealtUI;
    public Slider HealtBarSlider;

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
    public bool isAiming = false;
    public bool isShooting = false;

    [Header("Rotation")]
    [Range(0,10)]
    public float speed=7f;
    [Range(0, 5)]
    public float rotationSpeed = 4f;
    public Vector3 targetDirection = Vector3.zero;

    public int playerTeam;

    public UnityAction<bool> AimingFuction; 
    public UnityAction<bool> ShootingFuction;
    public UnityAction<bool> JumpingFuction;
    private void Awake()
    {
        References();
        AddListeners();
       
    }
    private void Start()
    {
        if(!photonView.IsMine)
        {
          
            Destroy(PlayerHealtUI);
        }
        if (photonView.Owner.CustomProperties.ContainsKey("Team"))
        {
            int team = (int)photonView.Owner.CustomProperties["Team"];
            playerTeam = team;
        }
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
        if (isJumping || isAiming || isShooting || shootingController.isReloading)
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
        if (isJumping || isAiming || isShooting || shootingController.isReloading)
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
        if (!photonView.IsMine)
            return;
        isAiming = value;
        animatorManager.PlayTargetAnimation("IsAiming", value);
    } 
    private void HandleShooting(bool value)
    {

        if (!photonView.IsMine)
            return;
        isShooting = value;
        animatorManager.PlayTargetAnimation("IsShooting", value);
    }
    public void ApplyDamage(float damage)
    {
        photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float  damage)
    {
        if (!photonView.IsMine)
            return;
        currentHealth -= damage;
        HealtBarSlider.value = currentHealth;
        if (currentHealth < 0)
            Die();
        Debug.Log(currentHealth + " Health");
        Debug.Log(damage + " Damage taken");
    }
    void Die()
    {
        playercontrolerManager.Die();
        ScoreBoard.instance.PlayerDied(playerTeam);
    }
    private void References()
    {
        currentHealth = maxHealth;
        cameraObject = Camera.main.transform;
        shootingController = GetComponent<ShootingController>();
        animatorManager = GetComponent<PlayerAnimatorManager>();
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        playercontrolerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerControlerManager>();
        HealtBarSlider.maxValue = 150f;
        HealtBarSlider.minValue = 0;
        HealtBarSlider.value = currentHealth;
    }
}
