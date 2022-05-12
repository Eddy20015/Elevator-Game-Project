using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Light1 : MonoBehaviourPunCallbacks
{
    //Tim Kashani

    [SerializeField] GameObject lightObject;

    [SerializeField] Light pointLight;

    [SerializeField] AudioSource audio;

    //temporary if I think of a better solution
    private GameObject player;
    private PhotonView playerView;

    float intensity;
    static float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        //debug

        multiplier = 1;

        player = GameObject.FindGameObjectWithTag("Player");
        audio.enabled = false;

        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players[0].GetPhotonView().IsMine && PhotonNetwork.IsMasterClient)
            {
                player = players[0];
                playerView = player.GetPhotonView();
            }
            else if(players[0].GetPhotonView().IsMine && !PhotonNetwork.IsMasterClient)
            {
                player = players[1];
                playerView = player.GetPhotonView();
            }
        }

        audio.enabled = false;

        intensity = pointLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (false)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 25)
            {
                //pointLight.SetActive(false);
            }
            else
            {
                //pointLight.SetActive(true);
            }
        }*/

        pointLight.intensity = intensity * multiplier;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Monster" && 
           /*(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE || GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PLAYING)*/
            GameStateManager.GetGameState() != GameStateManager.GAMESTATE.GAMEOVER)
        {
            lightObject.SetActive(false);
            audio.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster" &&
            /*(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE || GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PLAYING)*/
            GameStateManager.GetGameState() != GameStateManager.GAMESTATE.GAMEOVER)
        {
            StartCoroutine(TurnLightOn());
        }
    }

    public static void ChangeIntensity(float f)
    {
        //use a 0 to 1 scale for intensity
        multiplier = f;
    }

    void ChangeIntensity2()
    {
        //use a 0 to 1 scale for intensity
        //pointLight.intensity = intensity * multiplier;
    }

    IEnumerator TurnLightOn()
    {
        yield return new WaitForSeconds(3);
        lightObject.SetActive(true);
        audio.enabled = false;
    }
}
