using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;

    public Text blueTeamText;
    public Text redTeamText;

    public int blueTeamScore=0;
    public int redTeamScore=0;

    private PhotonView view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
        instance = this;
    }

    public void PlayerDied(int playerTeam)
    {
        if(playerTeam == 1)
        {
            redTeamScore++;
        }
        if(playerTeam == 2)
        {
            blueTeamScore++;
        }
        view.RPC("UpdateScores", RpcTarget.All, blueTeamScore, redTeamScore);
    }
    [PunRPC]
    void UpdateScores(int blueScore,int redScore)
    {
        blueTeamScore = blueScore;
        redTeamScore = redScore;

        blueTeamText.text = blueTeamScore.ToString();
        redTeamText.text = redTeamScore.ToString();
    }
}
