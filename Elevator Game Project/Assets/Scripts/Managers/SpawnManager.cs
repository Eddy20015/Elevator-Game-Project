using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{

    public static SpawnManager Instance;

    private static GameObject[] SpawnPoints;// = new Transform[2];

    public GameObject PlayerReference; // will be mainly for Local

    //set the static variables
    private void Awake()
    {
        Instance = new SpawnManager();

        SpawnPoints = new GameObject[2];
        SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    //Spawn the player using the local instantiation
    public void LocalSpawn()
    {
        GameObject Player = Instantiate(PlayerReference, SpawnPoints[0].transform);
        Player.GetComponentInChildren<Camera>().enabled = true;
    }

    //Spawn the player using the online instantiation
    public GameObject OnlineSpawn(bool Master, string PrefabName)
    {
        print(SpawnPoints[0]);

        //coded only for two
        if (Master)
        {
            return PhotonNetwork.Instantiate(PrefabName, SpawnPoints[0].transform.position, SpawnPoints[0].transform.rotation);
        }
        else
        {
            return PhotonNetwork.Instantiate(PrefabName, SpawnPoints[1].transform.position, SpawnPoints[1].transform.rotation);
        }
    }

}
