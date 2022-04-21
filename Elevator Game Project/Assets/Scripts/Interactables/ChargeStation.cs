using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChargeStation : MonoBehaviourPunCallbacks, IInteractable
{
    [SerializeField] private float chargedAmount, maxChargeAmount, incrementAmount;
    [SerializeField] private bool isUsed;
    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Interact();
        }
    }

    //Make sure isUsed is false once you are out of it
    private void OnTriggerExit(Collider other)
    {
        view.RPC("RPC_SetIsUsed", RpcTarget.Others, false);
    }

    public void Interact()
    {
        //**This will increase the charged amount
        //If it isn't fully charged + they are holding E + it isn't being used by another player
        if(chargedAmount < maxChargeAmount && Input.GetKey(KeyCode.E) && !isUsed)
        {
            view.RPC("RPC_SetIsUsed", RpcTarget.Others, true);
            chargedAmount += incrementAmount;
            view.RPC("RPC_SetChargedAmount", RpcTarget.Others, chargedAmount);
        }

        //If you let go of E, it is not in use **Makes it so only one person can use it at a time
        if(Input.GetKeyUp(KeyCode.E))
        {
            view.RPC("RPC_SetIsUsed", RpcTarget.Others, false);
        }
    }

    //Multiplayer code
    [PunRPC]
    void RPC_SetIsUsed(bool status)
    {
        isUsed = status;
    }

    [PunRPC]
    void RPC_SetChargedAmount(float amount)
    {
        chargedAmount = amount;
    }
}
