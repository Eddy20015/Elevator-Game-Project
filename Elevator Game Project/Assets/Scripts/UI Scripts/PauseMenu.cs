using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;

    public void OnClickResume()
    {
        GameStateManager.Play();
        pauseUI.SetActive(false);
    }

    public void OnClickMainMenu()
    {
        GameStateManager.MainMenu();
        pauseUI.SetActive(false);
    }
}
