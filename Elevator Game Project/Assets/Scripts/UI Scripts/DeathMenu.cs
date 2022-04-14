using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject deathUI;
    public void OnClickRestart()
    {
        GameStateManager.Restart();
        deathUI.SetActive(false);
    }

    public void OnClickMainMenu()
    {
        GameStateManager.MainMenu();
        deathUI.SetActive(false);
    }
}
