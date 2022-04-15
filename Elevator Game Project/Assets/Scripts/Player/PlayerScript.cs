using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //because the player is a one-shot kill, I just set their health to a boolean
    private bool isAlive = true;

    [SerializeField]
    private GameObject deathUI;

    [SerializeField]
    private GameObject pauseUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PLAYING)
        {
            GameStateManager.Pause();
            pauseUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PAUSE)
        {
            GameStateManager.Play();
            pauseUI.SetActive(false);
        }
    }

    private void Start()
    {
        deathUI.SetActive(false);
        pauseUI.SetActive(false);
    }

    public void GetKilled()
    {
        isAlive = false;
        GameStateManager.Gameover();
        deathUI.SetActive(true);
    }

    public void SetPauseUIToFalse()
    {
        pauseUI.SetActive(false);
    }
}
