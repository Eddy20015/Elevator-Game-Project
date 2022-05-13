using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryDetection : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryUI;
    [SerializeField] private ActivateElevator elevatorAnims;
    private int playersInArea = 0;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInArea += 1;
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL && ChargingStationManager.chargingStationManager.IsCompleted)
            {
                GameStateManager.Victory();
                victoryUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                //Elevator anims
                elevatorAnims.CloseDoors();
            }
            else if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && ChargingStationManager.chargingStationManager.IsCompleted)
            {
                if (playersInArea == 2)
                {
                    GameStateManager.Victory();
                    victoryUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    //Elevator anims
                    elevatorAnims.CloseDoors();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInArea -= 1;
        }
    }
}
