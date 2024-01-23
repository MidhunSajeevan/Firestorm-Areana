using UnityEngine;
using UnityEngine.Events;

public class ShootingController : MonoBehaviour
{
    [Header("Shooting Values")]
    public float fireRate = 0f;
    public float fireRange = 100f;
    public float fireDamage = 15f;
    public float nextFireTime = 0f;
    Transform firePoint;

    public UnityAction ShootingFunction;
    PlayerLocomotion playerLocomotion;
    void Start()
    {
        firePoint = Camera.main.transform;
        ShootingFunction += Shoot;
    }

    void Update()
    {

    }
    private void Shoot()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(firePoint.position,firePoint.forward,out hit, fireRange))
        {
            Vector3 hitPoint = hit.point;
            hitPoint.y = 0;
     

            // Set the player's rotation to face the hit point (instantaneous look)
            transform.LookAt(hitPoint);


            //Extract hit information
            //Apply damage to the player
        }
        // play muzle flash 
        // play sound
    }
}
