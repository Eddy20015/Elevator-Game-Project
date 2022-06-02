using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Photon.Pun;

public class IntroScene : MonoBehaviourPunCallbacks
{
    [SerializeField] private VideoClip RightVersion;
    [SerializeField] private VideoClip LeftVersion;
    [SerializeField] private VideoPlayer VidPlayer;
    [SerializeField] private SceneLoader Loader;
    [SerializeField] private GameObject BlackScreen;
    [SerializeField] private PhotonView view;

    private bool HasPlayed;

    //these are important for online
    private bool MasterComplete;
    private bool FollowComplete;

    void Awake()
    {
        GameStateManager.Cinematics();

        MasterComplete = false;
        FollowComplete = false;

        //if you game is online and you are the masterer, then play the right version. Otherwise, play the left version
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && PhotonNetwork.IsMasterClient)
        {
            VidPlayer.clip = RightVersion;
        }
        else
        {
            VidPlayer.clip = LeftVersion;
        }

        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            view = GetComponent<PhotonView>();
        }
    }

    private void Start()
    {
        VidPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //isPlaying doesnt start right away, so use HadStartedPlaying to determine if it is just lagging or actually the end of the clip
        if (VidPlayer.isPlaying)
        {
            BlackScreen.SetActive(false);
            HasPlayed = true;
        }
        else
        {
            BlackScreen.SetActive(true);
            if (HasPlayed)
            {
                VideoEnded();
            }
        }

        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            //when the game is online, make sure that both clips have ended before going to the next scene
            if (MasterComplete && FollowComplete)
            {
                Loader.LoadScene();
            }
        }
    }

    //will be called when the clip is over
    private void VideoEnded()
    {
        Debug.LogError("Got into checkover");
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            if(view.IsMine && PhotonNetwork.IsMasterClient)
            {
                view.RPC("RPC_MasterDone", RpcTarget.All);
            }
            else if(view.IsMine && !PhotonNetwork.IsMasterClient)
            {
                view.RPC("RPC_FollowDone", RpcTarget.All);
            }
        }
        //offline players just need to load the scene when they finish
        else
        {
            Loader.LoadScene();
        }
    }

    [PunRPC]
    private void RPC_MasterDone()
    {
        MasterComplete = true;
    }

    [PunRPC]
    private void RPC_FollowDone()
    {
        FollowComplete = true;
    }
}
