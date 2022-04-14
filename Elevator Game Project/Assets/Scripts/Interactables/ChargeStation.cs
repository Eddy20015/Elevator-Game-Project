using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ChargeStation : MonoBehaviour, IInteractable
{
    [SerializeField] private float chargedAmount, maxChargeAmount, chargeTick, incrementAmount;
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
    public void Interact()
    {
        //Only can be used by 1 per person
        if (isUsed)
            return;
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
        while(chargedAmount < maxChargeAmount && Input.GetKey(KeyCode.E))
        {
            chargedAmount += incrementAmount;
            yield return new WaitForSeconds(chargeTick);
            Debug.Log(chargedAmount);
            //Send the info to other clients
            if(view.IsMine)
            {
                view.RPC("RPC_SetIsUsed", RpcTarget.AllBuffered, isUsed);
                view.RPC("RPC_SetChargedAmount", RpcTarget.AllBuffered, chargedAmount);
            }
        }
        isUsed = false;
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
