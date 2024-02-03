using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;

    public ParticleSystem WinnerCelibration;
    Transform Celibrationposition;
    public Text blueTeamText;
    public Text redTeamText;

    public Text blueTeamWinText;
    public Text redTeamWinText;
    public Text WinnerName;

    public int blueTeamScore=0;
    public int redTeamScore=0;
    public bool WinnPopUpCalled=false;

    public GameObject WinnerUiPopUp;
    private PhotonView view;
    Vector3 offset;
    private void Awake()
    {

        offset = new Vector3(0, 10f, 10f);
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
        if (WinnPopUpCalled)
            return;
        blueTeamScore = blueScore;
        redTeamScore = redScore;

        blueTeamText.text = blueTeamScore.ToString();
        redTeamText.text = redTeamScore.ToString();
       
    }
    public void ShowPopUp()
    {
        Celibrationposition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        view.RPC("ShowWinPopUp", RpcTarget.All, blueTeamScore, redTeamScore);
    }
    [PunRPC]
    void ShowWinPopUp(int blueScore, int redScore)
    {
        if (WinnPopUpCalled)
            return;
        WinnPopUpCalled = true;
        WinnerUiPopUp.SetActive(true);
        blueTeamWinText.text = blueScore.ToString();
        redTeamWinText.text = redScore.ToString();

        if (blueScore > redScore)
        {
            if(view.IsMine)
            {
                WinnerName.text = "Your Team Won";
                WinnerName.color = Color.blue;
            }
            else
            {
                WinnerName.text = "Blue Team Victory You Lost";
                WinnerName.color = Color.blue;
            }
           
        }
        else if (blueScore < redScore)
        {
            if (!view.IsMine)
            {
                WinnerName.text = "Your Team Won";
                WinnerName.color = Color.red;
            }
            else
            {
                WinnerName.text = "Red Team Victory You Lost";
                WinnerName.color = Color.red;
            }
        }
        else
        {
            WinnerName.text = "Both Teams Won ";
        }
        
        PhotonNetwork.Instantiate("Confetti_Explosion", Celibrationposition.position+offset, Quaternion.identity);
    }
}
