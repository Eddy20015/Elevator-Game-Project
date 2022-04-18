using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChargeStation : MonoBehaviour, IInteractable
{
    [SerializeField] private float chargedAmount, maxChargeAmount, chargeTick, incrementAmount;
    [SerializeField] private bool isUsed, isCompleted;
    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        isCompleted = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Interact();
        }
    }
    public void Interact()
    {
        //check if all puzzles are completed
        if (isCompleted)
        {
            ChargingStationManager.chargingStationManager.CheckPuzzleState();
        }
        //Only can be used by 1 per person
        if (isUsed)
        {
            return;
        }  
        else
        {
            ChargeTheStation();
        }
    }

    public void ChargeTheStation()
    {
        StartCoroutine(ChargingStation());
    }

    //Charge the station until max only if they are holding down E
    public IEnumerator ChargingStation()
    {
        isUsed = true;
        while (chargedAmount < maxChargeAmount && Input.GetKey(KeyCode.E))
        {
            chargedAmount += incrementAmount;
            yield return new WaitForSeconds(chargeTick);
            Debug.Log(chargedAmount);
            //Send the info to other clients
            if (view.IsMine)
            {
                view.RPC("RPC_SetIsUsed", RpcTarget.AllBuffered, isUsed);
                view.RPC("RPC_SetChargedAmount", RpcTarget.AllBuffered, chargedAmount);
            }
        }
        isUsed = false;
        //Check if this station is completed
        if (chargedAmount >= maxChargeAmount)
        {
            isCompleted = true;
            if(view.IsMine)
            {
                view.RPC("RPC_SetCompleted", RpcTarget.AllBuffered, isCompleted);
            }
        }
    }

    public bool getPuzzleState()
    {
        return isCompleted;
    }

    [PunRPC]
    void RPC_SetCompleted(bool status)
    {
        isCompleted = status;
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
