using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //this is the script that handles the buttons in the main menu

    [SerializeField] private string SingleplayerLevel;

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
