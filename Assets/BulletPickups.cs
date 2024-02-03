using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickups : MonoBehaviour
{
    int AmoCount = 30;
    ShootingController controller;

    public void OnTriggerEnter(Collider other)
    {
        controller = other.transform.GetComponent<ShootingController>();
        if(controller != null )
        {
            controller.AddAmo( AmoCount );
            Destroy(gameObject);
        }
       
    }
}
