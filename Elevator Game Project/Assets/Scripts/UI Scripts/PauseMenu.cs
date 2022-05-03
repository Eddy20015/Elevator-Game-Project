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
        Cursor.lockState = CursorLockMode.Locked;
        pauseUI.GetComponent<Canvas>().enabled = false;
    }

    public void OnClickMainMenu()
    {
        GameStateManager.MainMenu();
        pauseUI.GetComponent<Canvas>().enabled = false;
    }
}
