using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserNameAndTeamNameDisplaye : MonoBehaviour
{

    public PhotonView view;
    public Text UserNameText;

    public Text teameText;
    void Start()
    {
        if(view.IsMine)
        {
            gameObject.SetActive(false);
        }
        UserNameText.text = view.Owner.NickName;
        if(view.Owner.CustomProperties.ContainsKey("Team"))
        {
            int team = (int)view.Owner.CustomProperties["Team"];
            teameText.text = "Team "+team.ToString();   
        }
    }

   
}
