using UnityEngine;


public class Boostups : MonoBehaviour
{
    PlayerLocomotion locomotion;
    private int health = 10;
    private void OnTriggerEnter(Collider other)
    {
      locomotion = other.GetComponent<PlayerLocomotion>();
        if(locomotion != null )
        {
            locomotion.ApplyHealth(health);
        }
        Destroy(gameObject);
    }
}
