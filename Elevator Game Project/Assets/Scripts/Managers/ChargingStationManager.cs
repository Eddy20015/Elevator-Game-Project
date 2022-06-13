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

    [SerializeField] public List<ChargeStation> chargeStations;
    [SerializeField] public bool isCompleted;
    private PhotonView view;
    [SerializeField] private float numOfCompletedStations, maxNumOfStations;

    public float NumOfCompletedStations { get => numOfCompletedStations; set => numOfCompletedStations = value; }
    public float MaxNumOfStations { get => maxNumOfStations; set => maxNumOfStations = value; }
    public bool IsCompleted { get => isCompleted; set => isCompleted = value; }

    // Start is called before the first frame update
    void Start()
    {
        numOfCompletedStations = 0;
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
        numOfCompletedStations = 0;
        bool temp = true;
        
        foreach (ChargeStation stations in chargeStations)
        {
            numOfCompletedStations++;
            if (stations.getPuzzleState() == false)
            {
                Debug.Log("GetPuzzleState() is false");
                numOfCompletedStations--;
                temp = false;
            }
            isCompleted = temp;
        }
        view.RPC("RPC_SyncNumbers", RpcTarget.All, numOfCompletedStations);
        //Updates all the clients
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        { 
            view.RPC("RPC_SetIsCompleted", RpcTarget.AllBuffered, isCompleted);
            view.RPC("ChangeIntensity", RpcTarget.All, numOfCompletedStations);
        } 
        else
        {
            ChangeIntensity(numOfCompletedStations);
        }


        //Debug.Log(isCompleted);



        //Light1.ChangeIntensity(Mathf.Lerp(1, 0.25f, numOfCompletedStations / 4));
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

    [PunRPC]
    void ChangeIntensity(float f)
    {
        foreach (Light1 l in FindObjectsOfType<Light1>())
        {
            l.ChangeIntensity2(Mathf.Lerp(1, 0.25f, f / 4));
        }
    }

    [PunRPC]
    private void RPC_SyncNumbers(float num)
    {
        numOfCompletedStations = num;
        //Debug.Log("In RPC and " + numOfCompletedStations + " and capital version is " + NumOfCompletedStations);
    }
}