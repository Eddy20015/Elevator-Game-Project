using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerScript : MonoBehaviourPunCallbacks
{
    //because the player is a one-shot kill, I just set their health to a boolean
    private bool isAlive = true;

    public bool JustRevived = false;

    private GameObject deathUI;

    private GameObject pauseUI;

    private GameObject victoryUI;

    [SerializeField]
    private Transform DeadPlayerTransform;

    [SerializeField]
    private string DeadPlayerName;

    private PhotonView view;

    private void Start()
    {
        GameObject[] Panels = GameObject.FindGameObjectsWithTag("Panel");

        //this needs to be editted if the order is changed
        //not great but good enough
        try{
            deathUI = Panels[0];
            pauseUI = Panels[1];
            victoryUI = Panels[2];

            //this will be needed by dead player
            deathUI.GetComponent<Canvas>().enabled = false;    //deathUI.SetActive(false);
            pauseUI.GetComponent<Canvas>().enabled = false;    //pauseUI.SetActive(false);

            //this wont be needed by dead player
            victoryUI.SetActive(false);
        }
        catch
        {
            //Do nothing
        }

        view = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        //used to revive the other views version of itself
        if (JustRevived)
        {
            JustRevived = false;
            view.RPC("RPC_Enable", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void RPC_Enable()
    {
        gameObject.SetActive(true);
    }

    //logic for pausing and unpausing
    private void Update()
    {
        if((GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL && GameStateManager.GetGameState() != GameStateManager.GAMESTATE.CINEMATIC) ||
          (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && view.IsMine))
        {
            if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PLAYING)
            {
                //OtherPauseMenu.CanDisplay = false;
                GameStateManager.Pause();
                Cursor.lockState = CursorLockMode.None;
                pauseUI.GetComponent<Canvas>().enabled = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PAUSE && pauseUI.activeInHierarchy == true)
            {
                GameStateManager.Play();
                Cursor.lockState = CursorLockMode.Locked;
                pauseUI.GetComponent<Canvas>().enabled = false;
            }

            //for online specifically
            else if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.CINEMATIC)
            {
                Cursor.lockState = CursorLockMode.Locked;
                pauseUI.GetComponent<Canvas>().enabled = false;
            }
        }

        if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.GAMEOVER)
        Debug.LogError(GameStateManager.GetGameState());
    }

    public void GetKilled()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            isAlive = false;

            VideoManager.SetJumpScare1(true);
        }
        else
        {
            Debug.Log("Killed");

            //this will create the new dead player at the current player's position then deactivate the player
            if (view.IsMine)
            {
                VideoManager.SetJumpScare1(false);

                GameObject DeadPlayer = PhotonNetwork.Instantiate(DeadPlayerName,
                    new Vector3(transform.position.x, 0.5f, transform.position.z),
                    DeadPlayerTransform.rotation);

                //sets the dead player to have access to the UI panels
                DeadPlayer DeadPlayerScript = DeadPlayer.GetComponent<DeadPlayer>();
                DeadPlayerScript.pauseUI = pauseUI;
                DeadPlayerScript.deathUI = deathUI;

                //allows the dead player object to know who the original was
                DeadPlayerScript.SetMyPlayer(gameObject);

                //sets the dead player cam to be current one used by everything
                Camera DeadPlayerCam = DeadPlayer.GetComponentInChildren<Camera>();
                DeadPlayerCam.enabled = true;
                gameObject.GetComponentInChildren<Camera>().enabled = false;

                view.RPC("RPC_Disable", RpcTarget.All, PhotonNetwork.IsMasterClient);
            }
        }
    }

    [PunRPC]
    public void RPC_Disable(bool WasMaster)
    {
        Debug.Log("In RPC, player is getting deactivated");

        if (WasMaster)
        {
            BuddySystemManager.Player1Died();
        }
        else
        {
            BuddySystemManager.Player2Died();
        }

        gameObject.SetActive(false);
    }

    public void SetPauseUIToFalse()
    {
        pauseUI.GetComponent<Canvas>().enabled = false;
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
