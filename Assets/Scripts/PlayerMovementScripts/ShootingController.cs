using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class ShootingController : MonoBehaviour
{
    [Header("Shooting Values")]
    public float fireRate = 0f;
    public float fireRange = 100f;
    public float fireDamage = 15f;
    public float nextFireTime = 0f;
    Transform firePoint;
    

    [Header("Reloading Values")]
    public int MaxAmo = 30;
    public int CurrentAmo;
    public float RelaodTime = 1.5f;

    [Header("Shooting Flags")]
    public bool isReloading = false;
    public bool isShooting = false;

    [Header("Shooting Sounds")]
    AudioSource audioSource;
    public AudioClip ShootingSound;
    public AudioClip ReloadingSound;

    [Header("Effects")]
    public ParticleSystem bloodEffect;

    public UnityAction ShootingFunction;
    public UnityAction ReloadingFunction;

    InputManager inputManager;
    PlayerAnimatorManager playerAnimatorManager;
    PhotonView photonView;

    public int playerTeam;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        photonView = GetComponent<PhotonView>();

       
    }
    void Start()
    {
        Referrences();



        if (photonView.Owner.CustomProperties.ContainsKey("Team"))
        {
            int team = (int)photonView.Owner.CustomProperties["Team"];
            playerTeam = team;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        if (CurrentAmo == 0)
        {
           
            Reload();
        }
    }
    private void Shoot()
    {
        if (!photonView.IsMine)
            return;
        if ((inputManager.movementInput == Vector2.zero && inputManager.Aiming == false) || isReloading)
        {
            return;
        }



        if (!isReloading)
        {
            RaycastHit hit;

            if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange))
            {
                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;
                hitPoint.y = 0;
                Debug.Log("Shooting" + hit.transform.name);

            
                transform.LookAt(hitPoint);
                CurrentAmo--;

                //Extract hit information
                //Apply damage to the player

                PlayerLocomotion playerDamage = hit.collider.GetComponent<PlayerLocomotion>();

                if(playerDamage != null && playerDamage.playerTeam != playerTeam)
                {
                    // Apply Damage
                    playerDamage.ApplyDamage(fireDamage);
                    // Set the player's rotation to face the hit point (instantaneous look)
                    photonView.RPC("RPC_Shoot", RpcTarget.All, hitPoint, hitNormal);
                }
            }
            // play muzle flash 
            audioSource.PlayOneShot(ShootingSound);
        }
        
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPoint,Vector3 hitNormal)
    {
        ParticleSystem blood = Instantiate(bloodEffect,hitPoint,Quaternion.LookRotation(hitNormal));
        Destroy(blood, blood.main.duration);
    }
    private void Reload()
    {
        if (!photonView.IsMine)
            return;
        if (CurrentAmo == MaxAmo)
            return;
        if (!isReloading && CurrentAmo < MaxAmo)
        {
           
            isReloading = true;
            //Play the relaod sound
            audioSource.PlayOneShot(ReloadingSound);
            if (inputManager.movementInput != Vector2.zero)
                playerAnimatorManager.animator.SetTrigger("RunningReload");
            else
                playerAnimatorManager.animator.SetTrigger("Reload");
            Invoke("FinishReloading", 2f);
        }
    }

    private void FinishReloading()
    {
        CurrentAmo = MaxAmo;
        isReloading = false;

    }
    private void Referrences()
    {
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        inputManager = GetComponent<InputManager>();
        firePoint = Camera.main.transform;
        ShootingFunction += Shoot;
        ReloadingFunction += Reload;
        CurrentAmo = MaxAmo;
    }
}
