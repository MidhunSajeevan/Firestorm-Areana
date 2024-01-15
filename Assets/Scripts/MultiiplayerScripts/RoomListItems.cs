using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListItems : MonoBehaviour
{
    [SerializeField] Text roomNameText;
    RoomInfo roomInfo;
   public void SetUp(RoomInfo _roomInfo)
    {
        roomInfo = _roomInfo;   
        roomNameText.text = _roomInfo.Name;
    }
    public void OnClick()
    {
            Launcher.Instance.JoinRoom(roomInfo);
    }
}
