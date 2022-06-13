using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class VictoryDetection : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject victoryUI;
    [SerializeField] private ActivateElevator elevatorAnims;
    [SerializeField] private GameObject invisibleCollider;
    [SerializeField] private FadingScript fadingScript;
    [SerializeField] private SceneLoader Loader;
    [SerializeField] private string NextLevelName;
    [SerializeField] private GameObject wall;
    [SerializeField] private ChargingStationManager charge;
    [SerializeField] private ChargeStation finalGen;
    [SerializeField] private GameObject open;
    [SerializeField] private Vector3 initialpos;
    [SerializeField] private float topen;
    [SerializeField] private float time;
    private int playersInArea = 0; 


    private void OnTriggerEnter(Collider other)
    {
        if (wall != null)
        {
            initialpos = wall.transform.position;
        }
        
        charge = GetComponent<ChargingStationManager>();
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
        bool DoIt = true;
        if (SceneManager.GetActiveScene().name == "Level 3" && ChargingStationManager.chargingStationManager.MaxNumOfStations == 5)
        {
            if(ChargingStationManager.chargingStationManager.NumOfCompletedStations != 5)
            {
                DoIt = false;
            }
        }

        if (DoIt)
        {
            //Activate the collider
            invisibleCollider.SetActive(true);

            yield return new WaitForSeconds(1f);
            //Close the elevators 
            elevatorAnims.CloseDoors();
            //GameStateManager.Cinematics();

            if (SceneManager.GetActiveScene().name == "Level 3")
            {
                //wall.transform.position = new Vector3(-95, 9, 51);

                yield return new WaitForSeconds(time);
                float elapsedTime = 0;
                while (elapsedTime < topen)
                {
                    if (wall != null)
                    {
                        wall.transform.position = Vector3.Lerp(initialpos, open.transform.position, elapsedTime / topen);
                        elapsedTime += Time.deltaTime;
                        yield return new WaitForSeconds(Time.deltaTime);
                    }
                }

                if (ChargingStationManager.chargingStationManager.MaxNumOfStations == 4)
                {
                    ChargingStationManager.chargingStationManager.MaxNumOfStations += 1;
                    wall.transform.position = open.transform.position;
                    charge.chargeStations.Add(finalGen);
                }
            }

            yield return new WaitForSeconds(3.5f);
            //Fade In
            fadingScript.FadeIn();

            yield return new WaitForSeconds(2.25f);

            //LoadTheNextLevel
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
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
