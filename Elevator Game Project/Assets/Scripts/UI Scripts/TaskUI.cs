using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject task;
    [SerializeField] private GameObject GoToElevator;
    private GameObject specificTask;
    private TextMeshProUGUI taskUI;
    private GameObject Background;

    private void Awake()
    {
        taskUI = task.GetComponent<TextMeshProUGUI>();
        Background = GameObject.Find("Background");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.CINEMATIC || GameStateManager.GAMESTATE.GAMEOVER == GameStateManager.GetGameState())
        {
            Background.SetActive(false);
        }
        else
        {
            Background.SetActive(true);
        }
    }

    public void UpdateUI()
    {
        if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.CINEMATIC)
        {
            GetComponent<Canvas>().enabled = false;
        }
        else
        {
            GetComponent<Canvas>().enabled = true;
        }
        if (taskUI != null)
        {
            taskUI.text = ChargingStationManager.chargingStationManager.NumOfCompletedStations.ToString("0") + "/" + ChargingStationManager.chargingStationManager.MaxNumOfStations.ToString("0");
            if(ChargingStationManager.chargingStationManager.NumOfCompletedStations == 4)
            {
                GoToElevator.SetActive(true);
            }
        }
    }
}
