
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
public class PlayerListItems : MonoBehaviourPunCallbacks
{
    public Text playerUserName;
    Player player;

    public void SetUp(Player _player)
    {
        player = _player;
        playerUserName.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(otherPlayer == player)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
