
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
public class PlayerListItems : MonoBehaviourPunCallbacks
{
    public Text playerUserName;
    public Text TeamText;
    Player player;
    int team;
    public void SetUp(Player _player,int _team)
    {
        
        //Display the player name on the text for the UI
        player = _player;
        team = _team;
        TeamText.text ="Team "+_team.ToString();
        playerUserName.text = _player.NickName;

        ExitGames.Client.Photon.Hashtable customProps =  new ExitGames.Client.Photon.Hashtable();
        customProps["Team"] = _team;
        _player.SetCustomProperties(customProps);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //After the player left the room destroy the object 
        if(otherPlayer == player)
        {
            Destroy(gameObject);
        }
    }
    public override void OnLeftRoom()
    {
        //on this player left the room destroy the name of this player
        Destroy(gameObject);
    }
}
