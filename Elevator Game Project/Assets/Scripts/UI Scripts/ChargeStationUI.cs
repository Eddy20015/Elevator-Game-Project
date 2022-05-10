using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeStationUI : MonoBehaviour
{
    [SerializeField] private ChargeStation chargeStation;
    [SerializeField] private Image chargedUI;
    // Start is called before the first frame update
    void Start()
    {
        chargeStation = GetComponent<ChargeStation>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        if(chargedUI != null)
            chargedUI.fillAmount = chargeStation.ChargedAmount/chargeStation.MaxChargeAmount;
    }
}
