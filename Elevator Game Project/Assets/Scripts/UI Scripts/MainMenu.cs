using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainMenu : MonoBehaviourPunCallbacks
{
    //this is the script that handles the buttons in the main menu

    [SerializeField] private string SingleplayerLevel;

    private bool OneFrame = false;

    private void Update()
    {
        //calls all of the photon leave room an disconnections
        if (!OneFrame)
        {
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
            {
                PhotonNetwork.LeaveRoom();
                Debug.LogError("LeaveRoom Called");
            }
            GameStateManager.NoPlayState();
            Cursor.lockState = CursorLockMode.None;
            OneFrame = true;
        }
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
        Debug.LogError("Disconnect Called");
    }

    //Start a New Local Game by sending player to the controller selector and changing the multiplay state to Local
    public void OnClickSingleplayer()
    {
        GameStateManager.Local();
        GameStateManager.Start(SingleplayerLevel);
    }

    //Enter the Lobby
    public void OnClickOnlineMultiplayer()
    {
        GameStateManager.Online();
        GameStateManager.ConnectToServerScene();
    }

    //Quit the Game
    public void OnClickQuitGame()
    {
        Application.Quit();
    }
}
