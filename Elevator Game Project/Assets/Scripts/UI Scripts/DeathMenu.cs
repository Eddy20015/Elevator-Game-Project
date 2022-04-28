using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject deathUI;

    private void Update()
    {
        if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.GAMEOVER)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void OnClickRestart()
    {
        GameStateManager.Restart();
        Cursor.lockState = CursorLockMode.Locked;
        deathUI.SetActive(false);
    }

    public void OnClickMainMenu()
    {
        GameStateManager.MainMenu();
        deathUI.SetActive(false);
    }
}
