using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Random_Position : MonoBehaviourPunCallbacks
{
    public Vector3[] positions;

    void Start()
    {
        int randomNumber = Random.Range(0, positions.Length);

        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            transform.position = positions[randomNumber];
        }
        else if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && PhotonNetwork.IsMasterClient)
        {
            transform.position = positions[randomNumber];
            GetComponent<PhotonView>().RPC("RPC_Sync", RpcTarget.Others, positions[randomNumber]);
        }
    }

    [PunRPC]
    private void RPC_Sync(Vector3 Placement)
    {
        transform.position = Placement;
    }
    
}
