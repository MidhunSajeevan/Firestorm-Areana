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
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerArmature"),Vector3.zero, Quaternion.identity);
    }
}
