using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    //public static RoomManager Instance;

    [SerializeField] private string GameSceneName;
    [SerializeField] private string PlayerManagerName;
    private GameObject playerManager;

    //Getters
    public GameObject PlayerManager { get => playerManager;}

    private void Awake()
    {
        //we only need this during an online scene
        if(GameStateManager.GetPlayState() != GameStateManager.PLAYSTATE.ONLINE)
        {
            Debug.LogError("PlayState is NOT ONLINE");
            SpawnManager.Instance.LocalSpawn();
            gameObject.SetActive(false);
        }
    }

    //Subscribe to a scene
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        //should I put a Online condition here?
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Check that scene that was subscribed and see if it is the right scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        //Check if we're in the game scene
        if(scene.name == GameSceneName)
        {
            Debug.LogError("Making the PlayerManagers");
            playerManager = PhotonNetwork.Instantiate(PlayerManagerName, Vector3.zero, Quaternion.identity);
        }
    }
}
