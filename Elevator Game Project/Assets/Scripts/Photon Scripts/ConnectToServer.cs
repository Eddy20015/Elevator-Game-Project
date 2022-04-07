using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMPro.TMP_InputField UsernameInput;
    [SerializeField] private TMPro.TMP_Text ButtonText;

    private bool Connecting = false;

    public void OnClickConnect()
    {
        if(UsernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = UsernameInput.text;
            ButtonText.text = "Connecting...";
            Connecting = true;
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        GameStateManager.Lobby();
    }

    public void OnClickBack()
    {
        if (!Connecting)
        {
            GameStateManager.MainMenu();
        }
    }
}
