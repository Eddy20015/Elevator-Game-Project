using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

//Written by Ed
public class PlayerManager : MonoBehaviourPunCallbacks//, IPunObservable
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private string PrefabName;
    //[SerializeField] private string MasterTag;
    //[SerializeField] private string PlayerTag;

    private PhotonView view;

    private GameStateManager.GAMESTATE LastState = GameStateManager.GAMESTATE.PLAYING;    

    private bool CinematicsFix;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        CinematicsFix = false;

        if (view.IsMine)
        {
            CreateController();
        }

        GameStateManager.Play();
    }

    private void CreateController()
    {
        bool Master = PhotonNetwork.IsMasterClient; 
        GameObject InstantiatedPlayer = SpawnManager.Instance.OnlineSpawn(Master, PrefabName);
        Debug.Log("Spawned in CreateController");
    }

    //this shares information between the two players on line
    /*public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.LogError("In PhotonSerializedView");

        if (view.IsMine)
        {
            if (stream.IsWriting)
            {
                Debug.LogError("In PhotonSerializedView if");

                //this section shares the GameState
                if (GameStateManager.GetGameState() != GameStateManager.GAMESTATE.MENU)
                {
                    stream.SendNext(GameStateManager.GetGameState());
                }
                else if (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.MENU)
                {
                    GameStateManager.SetGameState(GameStateManager.GAMESTATE.PLAYING);
                    stream.SendNext(GameStateManager.GetGameState());
                }
            }
            else
            {
                //this way you don't try to typecast the wrong thing
                object Received = stream.ReceiveNext();

                Debug.LogError("In PhotonSerializedView else");

                if (Received is GameStateManager.GAMESTATE)
                {
                    Debug.LogError("In PhotonSerializedView Received is GameStateManager");

                    //this section receives the GameState
                    //this is important for GAMEOVER, PAUSE, and PLAYING States, but NOT CINEMATIC or MENU                
                    GameStateManager.GAMESTATE ReceivedState = (GameStateManager.GAMESTATE)Received;
                    if (ReceivedState == GameStateManager.GAMESTATE.MENU)
                    {
                        GameStateManager.SetGameState(GameStateManager.GAMESTATE.PLAYING);
                    }
                    //this condition is just as a precaution if a non transferable state sneaks through
                    else if (ReceivedState != GameStateManager.GAMESTATE.CINEMATIC)
                    {
                        GameStateManager.SetGameState(ReceivedState);
                    }
                    else
                    {
                        //Do nothing with the ReceivedState if CINEMATIC
                    }
                }
            }
        }
    }*/

    //See if the RPC is cleaner
    
    /*private void Update()
    {
        if (view.IsMine)
        {
            //LastState is the state that the gamestatemanager was in last frame
            
            if(LastState != GameStateManager.GetGameState())
            {
                LastState = GameStateManager.GetGameState();
                if(GameStateManager.GetGameState() != GameStateManager.GAMESTATE.PAUSE ||
                   GameStateManager.GetGameState() != GameStateManager.GAMESTATE.CINEMATIC)
                {
                    view.RPC("MakeshiftStream", RpcTarget.All, GameStateManager.GetGameState());
                }
            }
            else
            {
                //do nothing (no need to update LastState since it is literally identical already)
            }

            //this sets the received state to be what the sentstate is
            //this sets the GameStateManager state to be the received state
        }
    }

    [PunRPC]
    private void MakeshiftStream(GameStateManager.GAMESTATE ReceivedState)
    {
        LastState = ReceivedState;
        GameStateManager.SetGameState(ReceivedState);
    }*/
}
