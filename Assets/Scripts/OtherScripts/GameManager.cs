using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject[] UIObjects;
    [SerializeField]
    GameObject PouseMenu;
    public string mainMenuSceneName = "MenuScene";  // Replace with your actual main menu scene name
    private List<Player> playersList = new List<Player>();
    // Start is called before the first frame update


    public void PousebuttonClicked()
    {
      
        PouseMenu.SetActive(true);
        foreach (GameObject go in UIObjects)
        {
            go.SetActive(false);
        }
    }


    public void LeaveRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Assign a new master client before leaving
            AssignNewMasterClient();
        }

        PhotonNetwork.LeaveRoom();
     

    }
    private void AssignNewMasterClient()
    {
        // Get the list of players in the room
        Player[] players = PhotonNetwork.PlayerList;

        // Choose a new master client (you can implement your logic for selecting a new master)
        Player newMaster = FindNewMaster(players);

        if (newMaster != null)
        {
            // Set the new master client
            PhotonNetwork.SetMasterClient(newMaster);
        }
    }
    private Player FindNewMaster(Player[] players)
    {
        // Implement your logic for choosing a new master client
        // For simplicity, we'll choose the first player who is not the current master

        foreach (Player player in players)
        {
            if (!player.IsMasterClient)
            {
                return player;
            }
        }

        return null;
    }
   
    public override void OnLeftRoom()
    {

        SceneManager.LoadScene("MenuScene");
    }

    public void ResumeClicked()
    {
       
        PouseMenu.SetActive(false);
        foreach (GameObject go in UIObjects)
        {
            go.SetActive(true);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
