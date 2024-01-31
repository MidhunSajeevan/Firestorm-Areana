using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MiniMapIconColor : MonoBehaviour
{
    
    PhotonView photonView;
    Material material;

    void Start()
    {
        photonView = GetComponentInParent<PhotonView>();
        material = GetComponent<MeshRenderer>().material;
        // Store the original color
    }

    void Update()
    {
        if(photonView == null)
            photonView = GetComponentInParent<PhotonView>();
        if (photonView.IsMine)
        {
            // If the PhotonView is owned by the local player, set the color to green
            material.color = Color.green;
        }
        else
        {
            // If the PhotonView is owned by another player, set the color to red
            material.color = Color.red;
        }
    }
}
