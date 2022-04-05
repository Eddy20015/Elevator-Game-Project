using UnityEngine;
using UnityEngine.SceneManagement;
//using Photon.Pun;

public class GameStateManager : MonoBehaviour //PunCallbacks
{
    //Coded by Ed Slee

    //Will be using this GameStateManager as a SceneManager that loads and changes scenes with the correct GameState

    private static GameStateManager Instance;

    public enum GAMESTATE
    {
        PLAYING,
        GAMEOVER,
        MENU,
        PAUSE
    }

    private static GAMESTATE State;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
    }

    //sets State to GAMEOVER
    public static void Gameover()
    {
        State = GAMESTATE.GAMEOVER;

        //this is so that the game freezes when it's gameover
        //can be removed if we don't like the look
        Time.timeScale = 0f;
    }
    public static GAMESTATE GetState()
    {
        return State;
    }

    //set State to MENU, Multiplay to NONE and Load Main Menu
    public static void MainMenu()
    {
        State = GAMESTATE.MENU;
        SceneManager.LoadScene(/*MainMenuName*/ "");
    }

    //sets State to PAUSE
    //if we choose to use this, only in local games
    public static void Pause()
    {
        State = GAMESTATE.PAUSE;

        //this is so that the game freezes when it's gameover
        //can be removed if we don't like the look
        Time.timeScale = 0f;
    }

    //starts the current scene again
    public static void Restart()
    {
        Start(SceneManager.GetActiveScene().name);
    }

    //set State to PLAYING
    //would be used to resume play after a pause
    public static void Play()
    {
        State = GAMESTATE.PLAYING;
        Time.timeScale = 1f;
    }

    //set State to PLAYING and load a/the level
    //will also be used to restart after a gameover
    //if there is only one level, we could make the string name a SerializeField instead of a parameter
    public static void Start(string Level)
    {

        State = GAMESTATE.PLAYING;

        //not sure if this is necessary just cuz this is for local
        SceneManager.LoadScene(Level);

        //this can be removed if Gameover() will not set timescale to 0
        Time.timeScale = 1f;
    }
}
