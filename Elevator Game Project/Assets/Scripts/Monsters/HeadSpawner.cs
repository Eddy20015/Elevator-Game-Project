using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeadSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private string HeadName;
    [SerializeField] private Vector3 MasterLocation;
    [SerializeField] private Vector3 FollowLocation;
    private GameObject[] HeadMonsters;
    private bool Found;

    // Start is called before the first frame update
    void Start()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            Destroy(gameObject);
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(HeadName, MasterLocation, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(HeadName, FollowLocation, Quaternion.identity);
            }
        }

        HeadMonsters = new GameObject[2];
    }

    // Update is called once per frame
    void Update()
    {
        if (!Found)
        {
            HeadMonsters = GameObject.FindGameObjectsWithTag("Monster");
        }
        if(HeadMonsters[0] != null && HeadMonsters[1] != null)
        {
            Found = true;

            //this disables the monster that doesn't follow this view's player
            if (HeadMonsters[0].GetComponent<PhotonView>().IsMine)
            {
                HeadMonsters[1].SetActive(false);
            }
            else
            {
                HeadMonsters[1].SetActive(true);
            }
        }
    }
}
