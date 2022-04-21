using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

//Written by Ed
public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private string PrefabName;
    //[SerializeField] private string MasterTag;
    //[SerializeField] private string PlayerTag;

    private PhotonView view;

    private bool TagsSetUp;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        TagsSetUp = false;

        if (view.IsMine)
        {
            CreateController();
        }
    }

    /*private void Start()
    {
        if (view.IsMine)
        {
            CreateController();
        }
    }*/

    private void CreateController()
    {
        bool Master = PhotonNetwork.IsMasterClient; 
        GameObject InstantiatedPlayer = SpawnManager.Instance.OnlineSpawn(Master, PrefabName);
        Debug.Log("Spawned in CreateController");

        /*if (PhotonNetwork.IsMasterClient)
        {
            InstantiatedPlayer.tag = MasterTag;
            view.RPC("RPC_SetMasterTag", RpcTarget.Others);
        }*/
    }

    //if doesn't work as intended, just use update
    /*[PunRPC]
    private void RPC_SetMasterTag()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag(PlayerTag);
        foreach (GameObject _Player in Players)
        {
            if (!_Player.GetComponent<PhotonView>().IsMine)
            {
                _Player.tag = MasterTag;
                TagsSetUp = true;
                break;
            }
        }
    }*/

    //this shares information between the two players on line
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //this section shares the GameState
            if(GameStateManager.GetGameState() != GameStateManager.GAMESTATE.MENU && 
               GameStateManager.GetGameState() != GameStateManager.GAMESTATE.DEAD)
            {
                stream.SendNext(GameStateManager.GetGameState());
            }
            else if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.MENU)
            {
                GameStateManager.SetGameState(GameStateManager.GAMESTATE.PLAYING);
            }
            else
            {
                //don't do anything for DEAD, we don't want to transfer that state
            }
        }
        else 
        {
            //this way you don't try to typecast the wrong thing
            object Received = stream.ReceiveNext();
            if(Received is GameStateManager.GAMESTATE)
            {
                //this section receives the GameState
                //this is important for GAMEOVER and PLAYING States, but NOT PAUSE, DEAD, or MENU
                if (GameStateManager.GetGameState() != GameStateManager.GAMESTATE.PAUSE &&
                    GameStateManager.GetGameState() != GameStateManager.GAMESTATE.DEAD)
                {
                    GameStateManager.GAMESTATE ReceivedState = (GameStateManager.GAMESTATE)Received;
                    if (ReceivedState == GameStateManager.GAMESTATE.MENU)
                    {
                        GameStateManager.SetGameState(GameStateManager.GAMESTATE.PLAYING);
                    }
                    //this condition is just as a precaution if a non transferable state sneaks through
                    else if (ReceivedState != GameStateManager.GAMESTATE.PAUSE &&
                             ReceivedState != GameStateManager.GAMESTATE.DEAD)
                    {
                        GameStateManager.SetGameState(ReceivedState);
                    }
                    else
                    {
                        //Do nothing with the ReceivedState if Paused or Dead
                    }
                }
            }
        }
    }

    //See if the RPC is cleaner
    /*
    private void Update()
    {
        //This is so that on the perspective of the nonMasterClient, the MasterClient has their MasterTag
        if (!PhotonNetwork.IsMasterClient && !TagsSetUp)
        {
            GameObject[] Players = GameObject.FindGameObjectsWithTag(PlayerTag);
            foreach(GameObject Playerr in Players)
            {
                if (!Playerr.GetComponent<PhotonView>().IsMine)
                {
                    Playerr.tag = MasterTag;
                    TagsSetUp = true;
                    break;
                }
            }
        }
    }*/
}
