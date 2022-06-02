using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PauseMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject pauseUI;

    private PhotonView view;

    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (GameStateManager.GetGameState() != GameStateManager.GAMESTATE.GAMEOVER)
        {
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
            {
                if(PhotonNetwork.PlayerList.Length < 2)
                {
                    OnClickMainMenu();
                }
            }
        }
        if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PAUSE)
        {
            AudioListener.pause = true;
        }
        else
        {
            AudioListener.pause = false;
        }
    }

    public void OnClickResume()
    {
        GameStateManager.Play();
        Cursor.lockState = CursorLockMode.Locked;
        pauseUI.GetComponent<Canvas>().enabled = false;
        AudioListener.pause = false;
    }

    public void OnClickMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        GameStateManager.MainMenu();
        pauseUI.GetComponent<Canvas>().enabled = false;
        AudioListener.pause = false;
    }
}
