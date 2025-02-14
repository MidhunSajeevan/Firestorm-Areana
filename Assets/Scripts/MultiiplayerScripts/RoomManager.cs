using System.Numerics;
using Photon.Pun;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instance;
  
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene,LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1 )
        {
            //if the first scene is loaded instantiate playrcontroler Manager

           PhotonNetwork.Instantiate( Path.Combine("PhotonPrefabs", "PlayerControlerManager"), UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity);
        }
    }
}
