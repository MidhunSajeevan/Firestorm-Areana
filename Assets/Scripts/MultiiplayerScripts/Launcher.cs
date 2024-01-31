using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;
    [SerializeField] InputField roomNameInputField;
    [SerializeField] Text roomNameText;
    [SerializeField] Text errorText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerListPrefab;
    [SerializeField] GameObject startButton;

    int nextTeamNumber = 1;
    private void Awake()
    {
        Instance = this;    
    }
    private void Start()
    {
        //Establish a connection between client to server 
        PhotonNetwork.ConnectUsingSettings();
    
    }
  
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master...");
        //To join the client in the networking lobby after establishing a connection
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
   
    public override void OnJoinedLobby()
    {
      
        MenuManager.instance.OpenMenu("UserNameMenu");
        Debug.Log("Joined in a Lobby");
       
    }
    public void CreateRoom()
    {
    
        //return if there is no room name is in the field
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        //Create a room with the name that given in the inpur field
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu("LoadingMenu");

    }

  

    public override void OnJoinedRoom()
    {
        
        MenuManager.instance.OpenMenu("RoomMenu");
        ///Display the room name on the UI 
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        //Store value of number of player joined in the room 
        Player[] player = PhotonNetwork.PlayerList;

        //Destroy the children of the player list content for removing repetetion after joined and left the room
        //it does't need to show the player name
        foreach (Transform transform in PlayerListContent)
        {
            Destroy(transform.gameObject);
        }

        //Display the player names on the UI from the player array
        foreach(Player playerItem in player)
        {
            int teamNumber = GetNextTeamNumber();
            Instantiate(PlayerListPrefab, PlayerListContent).GetComponent<PlayerListItems>().SetUp(playerItem, teamNumber);
        }
        //Show start button for only the master client 
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //When master client left the room assign another person as master and show the start button for that player
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Generation Failed "+ message;
        //When room cration failed show the error messages in the UI
        MenuManager.instance.OpenMenu("ErrorMenu");
        Debug.LogError(errorText.text);
    }
   
    public void StartGame()
    {
        //Start the Scene based on the build index
        PhotonNetwork.LoadLevel(1);
    }
    public void JoinRoom(RoomInfo info)
    {
        //Join the room when clicked on the UI showing in the roomcration Menu
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("LoadingMenu");
    }
    public void LeaveRoom()
    {
        //On Click Leave room Leave room and show loading menu
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("CreateRoomMenu");
        
    }
    public override void OnLeftRoom()
    {
        //After left the room show tittle menu
        MenuManager.instance.OpenMenu("TittleMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //To avoid a conflict while one of the player is left room and rejoin the room
        //it show previous names so destroy the children
        foreach (Transform transform in roomListContent)
        {
            Destroy(transform.gameObject);
        }
        //Show the list of rooms that crated 
        for(int i = 0;i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue; 
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItems>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        int teamNumber = GetNextTeamNumber();

        //When player entered the room instantiate player name in the UI
       GameObject playerItem =  Instantiate(PlayerListPrefab,PlayerListContent);
        playerItem.GetComponent<PlayerListItems>().SetUp(newPlayer,teamNumber);
    }

    private int GetNextTeamNumber()
    {
        int teamNumber = nextTeamNumber;
        nextTeamNumber = 3 - nextTeamNumber;
        return teamNumber;  
    }
}