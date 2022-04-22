using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuddySystemManager : MonoBehaviourPunCallbacks
{
    //Conditions for if the game will end
    private static bool Player1Dead = false;
    private static bool Player2Dead = false;

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

    // Update is called once per frame
    void Update()
    {

        //this will make the state GameOver when both players are Dead
        if(Player1Dead && Player2Dead)
        {
            GameStateManager.Gameover();
        }
        else
        {
            //Debug.Log("Game isn't over yet: Player1Dead = " + Player1Dead + " and Player2Dead = " + Player2Dead);
        }
    }     
}
