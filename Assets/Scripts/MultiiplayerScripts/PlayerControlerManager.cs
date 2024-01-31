using UnityEngine;
using Photon.Pun;
using System.IO;
using System.Collections.Generic;
using Photon.Realtime;

public class PlayerControlerManager : MonoBehaviourPunCallbacks
{
    PhotonView photonView;
    GameObject controller;
    public int playerTeam;  

    private Dictionary<int,int> playerTeams = new Dictionary<int,int>();
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
       if(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            playerTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            Debug.Log("Player's Team : "+ playerTeam);

        }

        AssignPlayersToSpownArea(playerTeam);
    }
    void AssignPlayersToSpownArea(int team)
    {
        GameObject spownArea1 = GameObject.Find("SpownArea1");
        GameObject spownArea2 = GameObject.Find("SpownArea2");

        if(spownArea1 == null || spownArea2 == null)
        {
            Debug.LogError("SpownArea is not found");
            return;
        }
        Transform spawnPoint = null;

        if(team == 1)
        {
            spawnPoint = spownArea1.transform.GetChild(Random.Range(0,spownArea1.transform.childCount));
        }
        if (team == 2)
        {
            spawnPoint = spownArea2.transform.GetChild(Random.Range(0, spownArea2.transform.childCount));
        }
        if(spawnPoint != null)
        {
            //Spown the player on the position with its defoult rotation
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { photonView.ViewID });
            Debug.Log("Player instantiated");
        }
        else
        {
            Debug.LogError("No Availabel Spawn points for " + team);
        }
       
    }
    void AssignTeamsToPlayers()
    {
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            if(player.CustomProperties.ContainsKey("Team"))
            {
                int team = (int)player.CustomProperties["Team"];
                playerTeams[player.ActorNumber] = team;
                Debug.Log(player.NickName + "'s Team : " + team);

                AssignPlayersToSpownArea(team);
            }
        }
    }
    public void Die()
    {
        PhotonNetwork.Destroy(controller);

        CreateControler();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newplayer)
    {
        AssignTeamsToPlayers();
    }

}
