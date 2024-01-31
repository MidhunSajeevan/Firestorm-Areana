using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerUserNameManager : MonoBehaviour
{
    [SerializeField] private InputField userNameInputFiled;
    [SerializeField] private Text errorMessage;

    private void Start()
    {
        if(PlayerPrefs.HasKey("userName"))
        {
            userNameInputFiled.text = PlayerPrefs.GetString("userName");
            PhotonNetwork.NickName = PlayerPrefs.GetString("userName");
        }    
    }
    public void PlayerUserNameInputValueChanged()
    {
        string userName = userNameInputFiled.text; 

        if(!string.IsNullOrEmpty(userName) && userName.Length <= 20)
        {
            PhotonNetwork.NickName = userName;  
            PlayerPrefs.SetString("userName", userName);
            errorMessage.text = "";
            MenuManager.instance.OpenMenu("TittleMenu");
        }
        else
        {
            errorMessage.text = "User Name must be not null or grater than 20 charectores";
        }
    }
}
