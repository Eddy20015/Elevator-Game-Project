using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks
{
    //because the player is a one-shot kill, I just set their health to a boolean
    private bool isAlive = true;


    [SerializeField]
    private GameObject deathUI;

    [SerializeField]
    private GameObject pauseUI;

    private PhotonView view;

    private void Start()
    {
        deathUI.SetActive(false);
        pauseUI.SetActive(false);

        view = GetComponent<PhotonView>();
    }

    //logic for pausing and unpausing
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

    public void GetKilled()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            isAlive = false;
            GameStateManager.Gameover();
            deathUI.SetActive(true);
        }
        else
        {
            //BuddySystemManager.SetDead(PhotonNetwork.IsMasterClient);
        }
    }

    public void SetPauseUIToFalse()
    {
        pauseUI.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Kill"))
        {
            Debug.Log("Works");
            GetKilled();
        }

        if (other.tag.Equals("Find"))
        {
            //other.GetComponentInParent<Monster>().SetPlayer(gameObject, true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Kill"))
        {
            GetKilled();
        }

        if (other.tag.Equals("Find"))
        {
            //other.GetComponentInParent<Monster>().SetPlayer(gameObject, false);
        }
    }
}
