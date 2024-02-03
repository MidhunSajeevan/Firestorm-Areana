using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListItems : MonoBehaviour
{
    [SerializeField] Text roomNameText;
    public  RoomInfo roomInfo;
   public void SetUp(RoomInfo _roomInfo)
    {
        //Display the Name of the room on the text to display in UI
        roomInfo = _roomInfo;   
        roomNameText.text = _roomInfo.Name;
       
    }
    public void OnClick()
    {
        //When clicked on the button join the room
        Launcher.Instance.JoinRoom(roomInfo);
    }
}
