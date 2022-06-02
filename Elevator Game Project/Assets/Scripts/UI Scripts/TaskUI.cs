using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUI : MonoBehaviour
{
    [SerializeField] private GameObject task;
    private GameObject specificTask;
    private TextMeshProUGUI taskUI;

    private void Awake()
    {
        taskUI = task.GetComponent<TextMeshProUGUI>();
    }

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
