using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeathMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject deathUI;

    private PhotonView view;

    private bool MasterLeft;
    private bool FollowerLeft;

    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
        GameStateManager.Play();
    }

    private void Update()
    {
        if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.GAMEOVER)
        {
            Cursor.lockState = CursorLockMode.None;

            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
            {
                if (PhotonNetwork.PlayerList.Length < 2)
                {
                    OnClickMainMenu();
                }
            }
        }
    }

    public void OnClickRestart()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            GameStateManager.Restart();
            Cursor.lockState = CursorLockMode.Locked;
            deathUI.GetComponent<Canvas>().enabled = false;
        }
        else if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            view.RPC("RPC_Restart", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_Restart()
    {
        //PhotonNetwork.automaticallySyncScene = true;
        GameStateManager.Restart();
        Cursor.lockState = CursorLockMode.Locked;
        VideoManager.BlackScreen(true);
    }

    public void OnClickMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        GameStateManager.MainMenu();
        deathUI.GetComponent<Canvas>().enabled = false;
    }
}
