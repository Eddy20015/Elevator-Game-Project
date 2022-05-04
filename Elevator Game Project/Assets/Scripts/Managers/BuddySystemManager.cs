using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuddySystemManager : MonoBehaviourPunCallbacks
{
    //Conditions for if the game will end
    private static bool Player1Dead = false;
    private static bool Player2Dead = false;

    public static bool P1FirstVideoDone = false;
    public static bool P2FirstVideoDone = false;
    public static bool SecondVideoDone = false;

    [SerializeField] private GameObject deathUI;

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
            }
        }
    }     
}
