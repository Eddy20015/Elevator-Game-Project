using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VictoryDetection : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject victoryUI;
    [SerializeField] private ActivateElevator elevatorAnims;
    [SerializeField] private GameObject invisibleCollider;
    [SerializeField] private FadingScript fadingScript;
    [SerializeField] private SceneLoader Loader;
    [SerializeField] private string NextLevelName;
    private int playersInArea = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInArea += 1;
            Debug.Log("ADDED");
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL && ChargingStationManager.chargingStationManager.IsCompleted)
            {
                StartCoroutine(EndGame());
            }
            else if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && ChargingStationManager.chargingStationManager.IsCompleted)
            {
                if (playersInArea == 2)
                {
                    StartCoroutine(EndGame());
                }
            }
        }
        Debug.Log(playersInArea);
    }

    private IEnumerator EndGame()
    {
        //Activate the collider
        invisibleCollider.SetActive(true);

        yield return new WaitForSeconds(1f);
        //Close the elevators 
        elevatorAnims.CloseDoors();
        //GameStateManager.Cinematics();

        yield return new WaitForSeconds(3.5f);
        //Fade In
        fadingScript.FadeIn();

        yield return new WaitForSeconds(2f);

        //LoadTheNextLevel
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            Loader.LoadScene();
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameStateManager.Start(NextLevelName);
            }
        }


        //Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playersInArea -= 1;
        }
    }
}
