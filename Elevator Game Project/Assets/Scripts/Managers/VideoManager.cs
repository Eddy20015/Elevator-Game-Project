using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;

public class VideoManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private VideoClip JumpScare1Setter;
    [SerializeField] private VideoClip JumpScare2Setter;
    [SerializeField] private VideoPlayer VidPlayerSetter;
    [SerializeField] private GameObject VidImageSetter;
    [SerializeField] private Texture2D FirstFrameImageSetter;
    [SerializeField] private Texture2D BlackScreenImageSetter;
    [SerializeField] private GameObject BlackScreenOtherImageSetter;

    private static VideoClip JumpScare1;
    private static VideoClip JumpScare2;
    private static VideoPlayer VidPlayer;
    private static GameObject VidImage;
    private static RawImage VidRawImage;
    private static Texture2D FirstFrameImage;
    private static Texture2D BlackScreenImage;
    private static GameObject BlackScreenOtherImage;

    [SerializeField] private GameObject GameOverPanel;

    private PhotonView view;

    private static Camera CurrentActiveCam;

    private static bool ChangeCam;

    //true if the video should be started
    private static bool StartVideo;

    //the video is playing (since isPlaying isn't working idk why)
    private static bool Playing;

    //true if the video finishes is complete
    private static bool Completed;

    //this is will play the other jumpscare after 1 is done
    private static bool AfterTheOther;

    //Set GameOver Menu to go up
    private static bool BringUpPanel;

    //if video play() is called
    private static bool PlayCalled;

    //true if player1 has finished their video
    private bool P1FirstVideoDone;

    //true if player2 has finished their video
    private bool P2FirstVideoDone;

    // Start is called before the first frame update
    void Awake()
    {
        //setting static variables 
        JumpScare1 = JumpScare1Setter;
        JumpScare2 = JumpScare2Setter;
        VidPlayer = VidPlayerSetter;
        VidImage = VidImageSetter;
        FirstFrameImage = FirstFrameImageSetter;
        BlackScreenImage = BlackScreenImageSetter;
        BlackScreenOtherImage = BlackScreenOtherImageSetter;

        view = gameObject.GetPhotonView();

        //setting up vid raw image texture
        VidRawImage = VidImage.GetComponent<RawImage>();
        VidRawImage.texture = FirstFrameImage;
        VidImage.SetActive(false);

        ChangeCam = false;
        StartVideo = false;
        Playing = false;
        Completed = false;
        AfterTheOther = false;
        BringUpPanel = false;
        PlayCalled = false;
    }

    // Turns off the video
    private void Update()
    {
        //just so that things may look a little cleaner and cover up ugly parts that should not be seen,
        //we will use the other black image to cover that all up
        if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.CINEMATIC)
        {
            BlackScreenOtherImage.SetActive(true);
        }
        else if(GameStateManager.GetGameState() != GameStateManager.GAMESTATE.GAMEOVER)
        {
            BlackScreenOtherImage.SetActive(false);
        }

        //this will set the targetCamera to be the DeadPlayer Camera received from SetCamera
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            if (ChangeCam && CurrentActiveCam != null)
            {
                VidPlayer.targetCamera = CurrentActiveCam;
                ChangeCam = false;
            }
            else if (CurrentActiveCam == null)
            {
                ChangeCam = true;
            }
        }

        Debug.LogError("P1FirstVideoDone is " + P1FirstVideoDone + " and P2FirstVideoDone is " + P2FirstVideoDone);
        if(P1FirstVideoDone && P2FirstVideoDone)
        {
            P1FirstVideoDone = false;
            P2FirstVideoDone = false;
            if (VidPlayer.isPlaying)
            {
                VidPlayer.Pause();
                Playing = false;
                PlayCalled = false;
                StartVideo = false;
            }
            VidPlayer.clip = null;

            SetJumpScare2();
        }

        //Debug.LogError(GameStateManager.GetGameState());
        if (StartVideo)
        {
            Debug.Log("Is it playing???? " + VidPlayer.isPlaying);

            //Online, it seems that the image will cover the video
            if (VidPlayer.isPlaying && GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
            {
                VidRawImage.enabled = false;
            }

            if (!Playing)
            {
                //when it is play hasn't been called, you must start the video
                if(!PlayCalled)
                {
                    VidImage.SetActive(true);
                    VidPlayer.Play();
                    Playing = true;
                    PlayCalled = true;
                    GameStateManager.Cinematics();
                }

                else
                {
                    Completed = true;
                    StartVideo = false;
                    //VidImage.SetActive(false);
                    PlayCalled = false;
                }
            }
            else
            {
                //checks if the clip is over
                VidPlayer.loopPointReached += CheckOver;
            }
        }

        //This means that Jumpscare1 just happened and Jumpscare2 must now happen
        else if (Completed && AfterTheOther)
        {
            Debug.LogWarning("AfterTheOther is true");

            SetJumpScare2();
            Completed = false;
            AfterTheOther = false;
        }

        //This means that Jumpscare2 just happened and the Panel needs to comeup
        else if (Completed && BringUpPanel)
        {
            Debug.LogWarning("BringUpPanel is true");

            if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
            {
                GameOverPanel.GetComponent<Canvas>().enabled = true;
                GameStateManager.Gameover();
                Completed = false;
                BringUpPanel = false;
            }
            else if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
            {
                //then in buddy system game over stuff will be handled
                BuddySystemManager.SecondVideoDone = true;
            }
        }

        //This means that Jumpscare1 just happened in multiplayer with no Jumpscare2 coming next
        else if (Completed)
        {
            Debug.LogWarning("Only Completed is true");

            if(BuddySystemManager.Player1GetDeadState() && BuddySystemManager.Player2GetDeadState())
            {
                //this is causing the both at the same time bug
                P1FirstVideoDone = true;
                P2FirstVideoDone = true;
            }
            else
            {
                if (BuddySystemManager.Player1GetDeadState())
                {
                    P1FirstVideoDone = true;
                }
                else if (BuddySystemManager.Player2GetDeadState())
                {
                    P2FirstVideoDone = true;
                }
                GameStateManager.Play();
                Completed = false;
                VidRawImage.enabled = true;
                VidImage.SetActive(false);
            }

            VidPlayer.clip = null;
            view.RPC("RPC_SetFirstVideoStatus", RpcTarget.All, P1FirstVideoDone, P2FirstVideoDone);
        }

        //this will set the BuddySystemVariables
    }

    [PunRPC]
    public void RPC_SetFirstVideoStatus(bool P1, bool P2)
    {
        P1FirstVideoDone = P1;
        P2FirstVideoDone = P2;
        if(P1 && P2)
        {
            VidImage.SetActive(true);
            VidRawImage.texture = BlackScreenImage;
        }
    }

    //Set the Clip to be Jumpscare1 and plays
    public static void SetJumpScare1(bool _AfterTheOther)
    {
        Debug.LogWarning("Video1ShouldPlay");
        VidRawImage.texture = FirstFrameImage;
        VidPlayer.clip = JumpScare1;
        VidImage.SetActive(true);
        AfterTheOther = _AfterTheOther;
        StartVideo = true;
    }

    //Set the Clip to be Jumpscare2 and plays
    public static void SetJumpScare2()
    {
        Debug.LogWarning("Video2ShouldPlay");
        VidRawImage.texture = BlackScreenImage;
        VidPlayer.clip = JumpScare2;
        VidImage.SetActive(true);
        VidImage.GetComponent<AudioSource>().clip = null;
        BringUpPanel = true;
        StartVideo = true;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        //Debug.LogWarning("Video Is Over");
        VidRawImage.texture = BlackScreenImage;
        Playing = false;
    }

    public static void SetCamera(Camera cam)
    {
        CurrentActiveCam = cam;
    }
}
