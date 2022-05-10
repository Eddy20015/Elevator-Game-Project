using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryUI;
    public void OnClickRestart()
    {
        GameStateManager.Restart();
        Cursor.lockState = CursorLockMode.Locked;
        victoryUI.SetActive(false);
    }

    public void OnClickMainMenu()
    {
        GameStateManager.MainMenu();
        victoryUI.SetActive(false);
    }
}
