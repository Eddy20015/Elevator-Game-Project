using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuddySystemManager : MonoBehaviourPunCallbacks
{
    //Conditions for if the game will end
    private static bool Player1Dead;
    private static bool Player2Dead;

    public static bool P1FirstVideoDone;
    public static bool P2FirstVideoDone;
    public static bool SecondVideoDone;

    [SerializeField] private GameObject deathUI;

    private void Start()
    {
        Player1Dead = false;
        Player2Dead = false;

        P1FirstVideoDone = false;
        P2FirstVideoDone = false;
        SecondVideoDone = false;
}

    public static void Player1Died()
    {
        Player1Dead = true;
    }

    public static void Player2Died()
    {
        Player2Dead = true;
    }

    public static void Player1Revived()
    {
        Player1Dead = false;
    }

    public static void Player2Revived()
    {
        Player2Dead = false;
    }

    public static bool Player1GetDeadState()
    {
        return Player1Dead;
    }
    public static bool Player2GetDeadState()
    {
        return Player2Dead;
    }

    // Update is called once per frame
    void Update()
    {

        //this will make the state GameOver when both players are Dead
        if(Player1Dead && Player2Dead)
        {
            //actually we are going to call 
            /*if(VideoManager.P1FirstVideoDoneSetter && P2FirstVideoDone)
            {
                VideoManager.SetJumpScare2();
            }*/
            if (SecondVideoDone)
            {
                GameStateManager.Gameover();

                //opening up the gameover panel here because the players will be deactivated
                deathUI.GetComponent<Canvas>().enabled = true;
                deathUI.GetComponent<DeathMenu>().ClientDisableRestart();
                Cursor.lockState = CursorLockMode.None;
            }
        }
        //Debug.LogError("Player1Dead " + Player1Dead + " Player2Dead " + Player2Dead);
    }     
}
