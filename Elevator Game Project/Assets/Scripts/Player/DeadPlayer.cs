using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeadPlayer : MonoBehaviourPunCallbacks, IInteractable
{
    [SerializeField] private float chargedAmount, maxChargeAmount, incrementAmount;
    [SerializeField] GameObject indicator;
    private GameObject OriginalPlayer;

    private PhotonView view;

    public GameObject pauseUI;
    public GameObject deathUI;

    public float ChargedAmount { get => chargedAmount; }
    public float MaxChargeAmount { get => maxChargeAmount; }

    bool interact;

    private void Start()
    {
        PhotonView view = gameObject.GetPhotonView();
        gameObject.GetComponentInChildren<Animator>().SetBool("Is Dead", true);
    }

    private void Update()
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
        else if (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.CINEMATIC)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseUI.GetComponent<Canvas>().enabled = false;
        }
        Debug.LogWarning(transform.rotation);

        if (interact)
        {
            indicator.SetActive(true);
        } else {
            indicator.SetActive(false);
        }

        interact = false;
    }

    //sets the variable OriginalPlayer
    public void SetMyPlayer(GameObject Player)
    {
        OriginalPlayer = Player;
    }

    //function that will be called by pushing E by it
    public void Interact()
    {
        interact = true;

        Debug.Log("Interact was called");
        if (chargedAmount < maxChargeAmount && Input.GetKey(KeyCode.E))
        {
            chargedAmount += incrementAmount * Time.deltaTime * 100;
            Debug.Log(chargedAmount);
        }

        if (chargedAmount >= maxChargeAmount)
        {
            Debug.Log("Done!");
            StartCoroutine(PlayAnim());
        }
    }

    public IEnumerator PlayAnim()
    {
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + .1f, transform.position.z);
        gameObject.GetComponentInChildren<Animator>().SetBool("Is Dead", false);
        yield return new WaitForSecondsRealtime(2.7f);
        Revive();
    }

    public void Revive()
    {
        Debug.Log("Revived");
        

        PhotonView view = gameObject.GetPhotonView();

        //The Master will be trying to revive P2 and vice versa on this end, will be RPCd later
        if (!PhotonNetwork.IsMasterClient)
        {
            BuddySystemManager.Player1Revived();
            VideoManager.PlayerRevived(true);
        }
        else
        {
            BuddySystemManager.Player2Revived();
            VideoManager.PlayerRevived(false);
        }

        //this will delete the dead player and reactivate the original functional alive player
        view.RPC("RPC_Enable", RpcTarget.Others);

        //this needs to be one the opposite view of the person that is reviving, because of the camera switching
        view.RPC("RPC_Revive", RpcTarget.Others, PhotonNetwork.IsMasterClient);
    }

    [PunRPC]
    public void RPC_Enable()
    {
        OriginalPlayer.GetComponent<PlayerScript>().JustRevived = true;
        OriginalPlayer.SetActive(true);
    }

    [PunRPC]
    public void RPC_Revive(bool WasMaster)
    {
        OriginalPlayer.GetComponentInChildren<Camera>().enabled = true;
        gameObject.GetComponentInChildren<Camera>().enabled = false;

        //The Master will be trying to revive P2 and vice versa
        if (!WasMaster)
        {
            BuddySystemManager.Player1Revived();
            VideoManager.PlayerRevived(true);
        }
        else
        {
            BuddySystemManager.Player2Revived();
            VideoManager.PlayerRevived(true);
        }

        PhotonNetwork.Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        /*if (!collision.gameObject.tag.Equals("Floor"))
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collision.gameObject.GetComponent<Collider>());
        }*/
    }
}
