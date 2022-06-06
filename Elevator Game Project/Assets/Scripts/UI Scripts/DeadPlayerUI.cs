using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadPlayerUI : MonoBehaviour
{
    [SerializeField] private DeadPlayer deadPlayer;
    [SerializeField] private Image reviveUIBar;
    [SerializeField] private GameObject reviveUI;
    private GameObject livingPlayer;
    // Start is called before the first frame update
    void Start()
    {
        deadPlayer = GetComponent<DeadPlayer>();
        livingPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        if (livingPlayer.activeInHierarchy)
        {
            reviveUI.transform.LookAt(livingPlayer.transform);
        }
    }

    public void UpdateUI()
    {
        if (reviveUIBar != null)
            reviveUIBar.fillAmount = deadPlayer.ChargedAmount / deadPlayer.MaxChargeAmount;
    }
}
