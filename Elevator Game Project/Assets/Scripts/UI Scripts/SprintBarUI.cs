using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SprintBarUI : MonoBehaviour
{
    private Slider Bar;
    private PlayerMovement MyPlayer;


    // Start is called before the first frame update
    void Start()
    {
        Bar = gameObject.GetComponent<Slider>();

        //trying to acces the PlayerMovement script, but must be done differently depending on PlayState
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            MyPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }
        else if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
            if (Players[0].GetPhotonView().IsMine)
            {
                Players[0].GetComponent<PlayerMovement>();
            }
            else
            {
                Players[1].GetComponent<PlayerMovement>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Here we will simply set the Bar's value to the StaminaProportion since it will always be between 0 and 1
        Bar.value = MyPlayer.GetStaminaProportion();
    }
}
