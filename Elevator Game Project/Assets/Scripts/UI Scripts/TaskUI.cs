using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private Text taskUI;

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (taskUI != null)
        {
            taskUI.text = ChargingStationManager.chargingStationManager.NumOfCompletedStations.ToString("0") + "/" + ChargingStationManager.chargingStationManager.MaxNumOfStations.ToString("0");
        }
    }
}
