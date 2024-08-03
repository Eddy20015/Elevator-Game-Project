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
    [SerializeField] private RenderTexture VideoTextureSetter;
    [SerializeField] private GameObject BlackScreenOtherImageSetter;

    private static VideoClip JumpScare1;
    private static VideoClip JumpScare2;
    private static VideoPlayer VidPlayer;
    private static GameObject VidImage;
    private static RawImage VidRawImage;
    private static Texture2D FirstFrameImage;
    private static Texture2D BlackScreenImage;
    private static RenderTexture VideoTexture;
    private static GameObject BlackScreenOtherImage;

    [SerializeField] private GameObject GameOverPanel;

    private PhotonView view;

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
    private static bool P1FirstVideoDone;

    //true if player2 has finished their video
    private static bool P2FirstVideoDone;

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
        VideoTexture = VideoTextureSetter;
        BlackScreenOtherImage = BlackScreenOtherImageSetter;

        view = gameObject.GetPhotonView();

        //setting up vid raw image texture
        VidRawImage = VidImage.GetComponent<RawImage>();
        VidRawImage.texture = FirstFrameImage;
        VidImage.SetActive(false);

        StartVideo = false;
        Playing = false;
        Completed = false;
        AfterTheOther = false;
        BringUpPanel = false;
        PlayCalled = false;
        P1FirstVideoDone = false;
        P2FirstVideoDone = false;

        //very important for restart if there is lag before update starts
        BlackScreen(true);
    }

    // Turns off the video
    private void Update()
    {
        //Debug.LogError("Completed is " + Completed + " Bring Up Panel is " + BringUpPanel);
        //just so that things may look a little cleaner and cover up ugly parts that should not be seen,
        //we will use the other black image to cover that all up
        if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.CINEMATIC)
        {
            BlackScreen(true);
        }
        else if(GameStateManager.GetGameState() != GameStateManager.GAMESTATE.GAMEOVER)
        {
            BlackScreen(false);
        }

        //Debug.LogError(GameStateManager.GetGameState());
        if (StartVideo)
        {
            //once the actual video is successfully playing, set the texture to be the VideoRenderTexture
            if (VidPlayer.isPlaying)
            { 
                VidRawImage.texture = VideoTexture;
            }

            //not the same thing as isPlaying... this checks if it hasn't been called/should be playing, but even when true, isplaying might lag a little and be false,
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
            //VidRawImage.texture = BlackScreenImage is already done in CheckOver
            Completed = false;
            AfterTheOther = false;
            VidPlayer.clip = null;

            //Debug.LogError("SetJumpScare2 is getting called" + Time.timeScale);

            SetJumpScare2();
        }

        //This means that Jumpscare2 just happened and the Panel needs to comeup
        else if (Completed && BringUpPanel)
        {
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
                VidImage.SetActive(false);
            }

            VidPlayer.clip = null;

            Debug.LogError("is view null?" + (view == null));
            view.RPC("RPC_SetFirstVideoStatus", RpcTarget.All, P1FirstVideoDone, P2FirstVideoDone);
        }

        //checks if both enemies have finished their videos (more like started truthfully)
        if (P1FirstVideoDone && P2FirstVideoDone)
        {
            P1FirstVideoDone = false;
            P2FirstVideoDone = false;
            VidImage.SetActive(true);

            //this matters if a death happened during another player's cutscene
            if (VidPlayer.isPlaying)
            {
                VidPlayer.Pause();
                Playing = false;
                PlayCalled = false;
                StartVideo = false;
            }
            VidPlayer.clip = null;

            //VidRawImage.texture = BlackScreenImage this is done inside of SetJumpScare2()

            SetJumpScare2();
        }
    }

    //this will set the DoneVariables on both computers

    [PunRPC]
    public void RPC_SetFirstVideoStatus(bool P1, bool P2)
    {
        P1FirstVideoDone = P1;
        P2FirstVideoDone = P2;
    }

    //Set the Clip to be Jumpscare1 and plays
    public static void SetJumpScare1(bool _AfterTheOther)
    {
        Debug.LogWarning("Video1ShouldPlay");
        VidRawImage.texture = FirstFrameImage;
        VidPlayer.clip = JumpScare1;
        VidPlayer.SetDirectAudioMute(0, true);
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
        VidPlayer.SetDirectAudioMute(0, false);
        VidImage.SetActive(true);
        VidImage.GetComponent<AudioSource>().clip = null;
        BringUpPanel = true;
        StartVideo = true;
    }

    public static void PlayerRevived(bool P1)
    {
        if (P1)
        {
            P1FirstVideoDone = false;
        }
        else
        {
            P2FirstVideoDone = false;
        }
    }

    public static void BlackScreen(bool Enable)
    {
        BlackScreenOtherImage.SetActive(Enable);
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        VidRawImage.texture = BlackScreenImage;
        Playing = false;
    }
}
