using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerControlerManager : MonoBehaviour
{
    PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
       if(photonView.IsMine)
        {
            CreateControler();
        }
    }
    void CreateControler()
    {
        //Spown the player on the position with its defoult rotation
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerArmature"),Vector3.zero, Quaternion.identity);
    }
}
