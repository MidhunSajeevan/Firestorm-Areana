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
        MenuManager.instance.OpenMenu("TittleMenu");
        Debug.Log("Joined in a Lobby");
        PhotonNetwork.NickName = "Player"+Random.Range(0,1000).ToString();
    }
    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu("LoadingMenu");

    }
    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("RoomMenu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] player = PhotonNetwork.PlayerList;

        foreach (Transform transform in PlayerListContent)
        {
            Destroy(transform.gameObject);
        }

        foreach(Player playerItem in player)
        {
            Instantiate(PlayerListPrefab, PlayerListContent).GetComponent<PlayerListItems>().SetUp(playerItem);
        }
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {

        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Generation Failed "+ message;
        MenuManager.instance.OpenMenu("ErrorMenu");
        Debug.LogError(errorText.text);
    }
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("LoadingMenu");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("LoadingMenu");
    }
    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("TittleMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform transform in roomListContent)
        {
            Destroy(transform.gameObject);
        }
        for(int i = 0;i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItems>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListPrefab,PlayerListContent).GetComponent<PlayerListItems>().SetUp(newPlayer);
    }
}