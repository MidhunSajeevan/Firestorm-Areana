using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boostups : MonoBehaviour
{
    [SerializeField] Text boostup;
    int boostups = 0;
    private void OnTriggerEnter(Collider other)
    {
        boostups++;
        if(other != null)
        {
            boostup.text = boostups.ToString();
            Destroy(gameObject);
        }
    }
}
