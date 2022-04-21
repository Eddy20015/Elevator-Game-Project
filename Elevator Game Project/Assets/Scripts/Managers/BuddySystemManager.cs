using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuddySystemManager : MonoBehaviourPunCallbacks, IPunObservable
{
    //Conditions for if the game will end
    private static bool Player1Dead = false;
    private static bool Player2Dead = false;

    //Conditions for which view this BuddySystemManager is on
    private bool ThisIsMaster;
    private PhotonView view;

    //Condition must true to change the 
    private static bool NeedToChangeLivingState = false;

    //Player that has this view as theirs
    private GameObject Player;

    //Gameobject that depicts dead player
    [SerializeField] private string DeadPrefabName;
    [SerializeField] private GameObject DeadPrefabReference;
    private GameObject DeadPlayer;
    //private GameObject OtherDeadPlayer;

    void Start()
    {
        ThisIsMaster = PhotonNetwork.IsMasterClient;
        view = GetComponent<PhotonView>();

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //this will make the state GameOver when both players are Dead
        if(Player1Dead && Player2Dead)
        {
            GameStateManager.Gameover();
        }

        //this will call Alive or Dead depending on any changes to the Player1Dead or Player2Dead bools
        if (NeedToChangeLivingState)
        {
            if (ThisIsMaster)
            {
                if (Player1Dead)
                {
                    Debug.LogError("P1 Dead is called");
                    Dead();
                }
                else
                {
                    Alive();
                }
            }
            else
            {
                if (Player2Dead)
                {
                    Debug.LogError("P2 Dead is called");
                    Dead();
                }
                else
                {
                    Alive();
                }
            }
            NeedToChangeLivingState = false;
        }
    }     


    //this function will set Player1Dead or Player2Dead to true
    public static void SetDead(bool Master)
    {
        if (Master)
        {
            Player1Dead = true;
        }
        else
        {
            Player2Dead = true;
        }
        NeedToChangeLivingState = true;
    }

    //this function will set Player1Dead or Player2Dead to false
    public static void SetAlive(bool Master)
    {
        if (Master)
        {
            Player1Dead = false;
        }
        else
        {
            Player2Dead = false;
        }
        NeedToChangeLivingState = true;
    }

    //this will do the things that need to happen to the body when it is dead
    //such as making the player invisible and spawning a dead body
    private void Dead()
    {
        if(Player == null)
        {
            Debug.LogError("Player is null");
        }
        Player.GetComponentInChildren<MeshRenderer>().enabled = false;
        DeadPlayer = PhotonNetwork.Instantiate(DeadPrefabName, Player.transform.position, DeadPrefabReference.transform.rotation);
        

        //^That instantiation should spawn them on both scenes, but if not, then we do it again in the RPC

        PhotonView PlayerView = Player.GetComponent<PhotonView>();
        int ID = -1 + PlayerView.ViewID;

        view.RPC("RPC_Dead", RpcTarget.Others, ID);
    }

    [PunRPC]
    private void RPC_Dead(int ID)
    {
        GameObject OtherPlayer = PhotonView.Find(ID).gameObject;

        OtherPlayer.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    //this will do the things that need to happen to the body when it is alive
    //such as making the player visible, teleporting them back to their death spot, and destroying their dead body
    private void Alive()
    {
        Player.GetComponentInChildren<MeshRenderer>().enabled = true;
        Player.transform.position = DeadPlayer.transform.position;

        PhotonView PlayerView = Player.GetComponent<PhotonView>();
        int ID = -1 + PlayerView.ViewID;

        view.RPC("RPC_Alive", RpcTarget.Others, ID, 
                DeadPlayer.transform.position.x, 
                DeadPlayer.transform.position.y, 
                DeadPlayer.transform.position.z);

        PhotonNetwork.Destroy(DeadPlayer);
    }

    [PunRPC]
    private void RPC_Alive(int ID, float x, float y, float z)
    {
        GameObject OtherPlayer = PhotonView.Find(ID).gameObject;

        OtherPlayer.GetComponentInChildren<MeshRenderer>().enabled = false;
        OtherPlayer.transform.position = new Vector3(x, y, z);

        //PhotonNetwork.Destroy(DeadPlayer);
    }

    //this will be used to make sure that the static Player1Dead and Player2Dead are aligned
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Master will send the Player1Dead, as master is Player1
            if (ThisIsMaster)
            {
                stream.SendNext(Player1Dead);
            }
            //Not Master will send the Player2Dead, as not master is Player2
            else
            {
                stream.SendNext(Player2Dead);
            }
        }
        else
        {
            object Received = stream.ReceiveNext();
            if (Received is bool)
            {
                bool PlayerStatus = (bool) Received;

                //we want Master to have the PlayerStatus from NotMaster's Player2
                if (ThisIsMaster)
                {
                    Player2Dead = PlayerStatus;
                }

                //we want NotMaster to have the PlayerStatus from Master's Player 1
                else if(!ThisIsMaster)
                {
                    Player1Dead = PlayerStatus;
                }
            }
            else
            {
                Debug.LogError("BuddySystemReceived didn't received a bool");
            }
        }
    }
}
