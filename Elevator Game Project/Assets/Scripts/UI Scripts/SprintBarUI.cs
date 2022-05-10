using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SprintBarUI : MonoBehaviourPunCallbacks
{
    private Slider Bar;
    private PlayerMovement MyPlayer;

    private bool NotMyView = false;

    // Start is called before the first frame update
    void Start()
    {
        Bar = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(MyPlayer == null || NotMyView)
        {
            //trying to acces the PlayerMovement script, but must be done differently depending on PlayState
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
            {
                MyPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            }
            else if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
            {
                GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");

                //i know this is kinda a sin, but it is only for the first frame... gimme a break
                for(int i = 0; i < Players.Length; i++)
                {
                    Camera cam = Players[i].GetComponentInChildren<Camera>();

                    //identify the correct player based on if its camera is enabled
                    if (cam != null && cam.enabled == true)
                    {
                        MyPlayer = Players[i].GetComponent<PlayerMovement>();
                    }
                }
                //if no camera is enabled for the other photon player object, set this bool so it doesn't need to redo the if
                if(MyPlayer == null)
                {
                    NotMyView = true;
                }
            }
        }

        //Here we will simply set the Bar's value to the StaminaProportion since it will always be between 0 and 1
        if (MyPlayer != null)
        {
            Bar.value = MyPlayer.GetStaminaProportion();
        }
    }
}
