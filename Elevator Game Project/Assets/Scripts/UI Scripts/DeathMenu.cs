using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    public void OnClickRestart()
    {
        GameStateManager.Restart();
    }

    public void OnClickMainMenu()
    {
        GameStateManager.MainMenu();
    }
}
