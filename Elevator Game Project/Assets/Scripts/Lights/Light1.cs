using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Light1 : MonoBehaviourPunCallbacks
{
    //Tim Kashani

    [SerializeField] GameObject l;

    [SerializeField] GameObject pointLight;

    //temporary if I think of a better solution
    private GameObject player;
    private PhotonView playerView;

    // Start is called before the first frame update
    void Start()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (playerView != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 100)
            {
                pointLight.SetActive(false);
            }
            else
            {
                pointLight.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Monster")
        {
            l.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            StartCoroutine(TurnLightOn());
        }
    }

    IEnumerator TurnLightOn()
    {
        yield return new WaitForSeconds(3);
        l.SetActive(true);
    }
}
