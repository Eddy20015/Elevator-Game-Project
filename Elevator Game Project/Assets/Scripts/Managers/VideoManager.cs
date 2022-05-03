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
    [SerializeField] private GameObject JumpScare1AudioSetter;
    [SerializeField] private Texture2D FirstFrameImageSetter;
    [SerializeField] private Texture2D BlackScreenImage;

    private static VideoClip JumpScare1;
    private static VideoClip JumpScare2;
    private static VideoPlayer VidPlayer;
    private static GameObject VidImage;
    private static RawImage VidRawImage;
    private static Texture2D FirstFrameImage;
    //private static GameObject JumpScare1Audio;

    [SerializeField] private GameObject GameOverPanel;

    //true if the video should be started
    private static bool StartVideo;

    //the video is playing (since isPlaying isn't working idk why)
    private static bool Playing;

    //true if the ienumerator is complete
    private static bool Completed;

    //this is will play the other jumpscare after 1 is done
    private static bool AfterTheOther;

    //Set GameOver Menu to go up
    private static bool BringUpPanel;

    //if video play() is called
    private static bool PlayCalled;

    // Start is called before the first frame update
    void Awake()
    {
        //setting static variables
        JumpScare1 = JumpScare1Setter;
        JumpScare2 = JumpScare2Setter;
        VidPlayer = VidPlayerSetter;
        VidImage = VidImageSetter;
        FirstFrameImage = FirstFrameImageSetter;
        //JumpScare1Audio = JumpScare1AudioSetter;

        //setting up vid raw image texture
        VidRawImage = VidImage.GetComponent<RawImage>();
        VidRawImage.texture = FirstFrameImage;
        VidImage.SetActive(false);
        //JumpScare1Audio.SetActive(false);

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
        if (StartVideo)
        {
            Debug.Log("Is it playing???? " + VidPlayer.isPlaying);
            if (!Playing)
            {
                //when it is play hasn't been called, you must start the video
                if(!PlayCalled)
                {
                    VidImage.SetActive(true);
                    //JumpScare1Audio.SetActive(true);
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
                    //JumpScare1Audio.SetActive(false);
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
                SetJumpScare2();
            }
            else
            {
                GameStateManager.Play();
                Completed = false;
                VidImage.SetActive(false);
            }
        }
    }

    //Set the Clip to be Jumpscare1 and plays
    public static void SetJumpScare1(bool _AfterTheOther)
    {
        Debug.LogWarning("Video1ShouldPlay");
        VidRawImage.texture = FirstFrameImage;
        VidPlayer.clip = JumpScare1;
        AfterTheOther = _AfterTheOther;
        StartVideo = true;
    }

    //Set the Clip to be Jumpscare2 and plays
    public static void SetJumpScare2()
    {
        Debug.LogWarning("Video2ShouldPlay");
        VidPlayer.clip = JumpScare2;
        BringUpPanel = true;
        StartVideo = true;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.LogWarning("Video Is Over");
        VidRawImage.texture = BlackScreenImage;
        Playing = false;
    }
}
