using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //because the player is a one-shot kill, I just set their health to a boolean
    private bool isAlive = true;

    [SerializeField]
    private GameObject deathUI;

    private void Start()
    {
        deathUI.SetActive(false);
    }

    public void GetKilled()
    {
        isAlive = false;
        GameStateManager.Gameover();
        deathUI.SetActive(true);
    }
}
