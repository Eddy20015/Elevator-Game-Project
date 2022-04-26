using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeadPlayer : MonoBehaviourPunCallbacks, IInteractable
{
    private GameObject OriginalPlayer;

    private PhotonView view;

    private void Start()
    {
        PhotonView view = gameObject.GetPhotonView();
    }

    //sets the variable OriginalPlayer
    public void SetMyPlayer(GameObject Player)
    {
        OriginalPlayer = Player;
    }

    //function that will be called by pushing E by it
    public void Interact()
    {
        Debug.Log("Interact was called");
        Revive();
    }

    public void Revive()
    {
        Debug.Log("Revived");
        PhotonView view = gameObject.GetPhotonView();

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
        }
        else
        {
            BuddySystemManager.Player2Revived();
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