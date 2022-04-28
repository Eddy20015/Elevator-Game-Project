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

    private void Start()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            //Thinking that I will actually not be using instantiate to spawn the player
            //Instead I will add a script to the player in the scene that destroys it if it is online
            //this will disable the manager and properly spawn the player in local
            
            //Debug.Log("PlayState is LOCAL");
            //SpawnManager.Instance.LocalSpawn();
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
            playerManager = PhotonNetwork.Instantiate(PlayerManagerName, Vector3.zero, Quaternion.identity);
        }
    }
}
