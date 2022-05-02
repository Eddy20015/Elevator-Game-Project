using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Photon.Pun;

public class VideoManager : MonoBehaviourPunCallbacks
{
    //public static VideoManager Instance;

    [SerializeField] private VideoClip JumpScare1Setter;
    [SerializeField] private VideoClip JumpScare2Setter;
    [SerializeField] private VideoPlayer VidPlayerSetter;
    [SerializeField] private GameObject VidImageSetter;

    private static VideoClip JumpScare1;
    private static VideoClip JumpScare2;
    private static VideoPlayer VidPlayer;
    private static GameObject VidImage;

    [SerializeField] private GameObject GameOverPanel;

    //true if the coroutine should be started
    private static bool StartVideo;

    //the video is playing (since isPlaying isn't working idk why)
    private static bool Playing;

    //true if the ienumerator is complete
    private static bool Completed;

    //this is will play the other jumpscare after 1 is done
    private static bool AfterTheOther;

    //Set GameOver Menu to go up
    private static bool BringUpPanel;

    private static bool TempBool;

    // Start is called before the first frame update
    void Awake()
    {
        //Instance = new VideoManager();
        JumpScare1 = JumpScare1Setter;
        JumpScare2 = JumpScare2Setter;
        VidPlayer = VidPlayerSetter;
        VidImage = VidImageSetter;

        VidImage.SetActive(false);

        StartVideo = false;
        Playing = false;
        Completed = false;
        AfterTheOther = false;
        BringUpPanel = false;

        TempBool = false;
    }

    // Turns off the video
    private void Update()
    {
        if (StartVideo)
        {
            Debug.Log("Is it playing???? " + VidPlayer.isPlaying);
            if (!Playing)
            {
                //when it is inactive, you must start the video
                if(!TempBool)
                {
                    //VidImage.SetActive(true);
                    VidPlayer.Play();
                    Playing = true;
                    TempBool = true;
                }
                //when it is active, you must end the video
                else
                {
                    Completed = true;
                    StartVideo = false;
                    //VidImage.SetActive(false);
                    TempBool = false;

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

            GameOverPanel.SetActive(true);
            GameStateManager.Gameover();
            Completed = false;
            BringUpPanel = false;
        }

        //This means that Jumpscare1 just happened in multiplayer with no Jumpscare2 coming next
        else if (Completed)
        {
            Debug.LogWarning("Only Completed is true");

            Completed = false;
            VidImage.SetActive(false);
        }
    }

    //Set the Clip to be Jumpscare1 and plays
    public static void SetJumpScare1(bool _AfterTheOther)
    {
        Debug.LogWarning("Video1ShouldPlay");
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
        Playing = false;
    }
}
