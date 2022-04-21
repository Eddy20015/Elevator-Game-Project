using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// In charge of checking if all charging station puzzles are completed
/// </summary>
public class ChargingStationManager : MonoBehaviour
{
    //Static variable that can be called upon to check the status
    public static ChargingStationManager chargingStationManager;

    [SerializeField] private List<ChargeStation> chargeStations;
    private bool isCompleted;
    private PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (chargingStationManager == null)
        {
            chargingStationManager = this;
        }
        isCompleted = false;
    }

    //Checks if all the puzzles are completed
    public void CheckPuzzleState()
    {
        bool temp = true;
        foreach (ChargeStation stations in chargeStations)
        {
            if (stations.getPuzzleState() == false)
            {
                temp = false;
            }
            isCompleted = temp;
        }
        //Updates all the clients
        view.RPC("RPC_SetIsCompleted", RpcTarget.AllBuffered, isCompleted);
        

        //Debug.Log(isCompleted);
    }

    //Gets the puzzle state, if completed, then all puzzles are completed
    public bool GetPuzzleState()
    {
        return isCompleted;
    }

    [PunRPC]
    void RPC_SetIsCompleted(bool status)
    {
        isCompleted = status;
    }
}