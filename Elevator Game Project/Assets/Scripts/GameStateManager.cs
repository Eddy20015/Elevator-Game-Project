using UnityEngine;
using UnityEngine.SceneManagement;
//using Photon.Pun;

public class GameStateManager : MonoBehaviour //PunCallbacks
{
    //Coded by Ed Slee

    //Will be using this GameStateManager as a SceneManager that loads and changes scenes with the correct GameState

    private static GameStateManager Instance;

    //this is what kind of state the game is in itself
    public enum GAMESTATE
    {
        PLAYING,
        GAMEOVER,
        MENU,
        PAUSE
    }

    private static GAMESTATE GameState;

    //this is the current state of whether it is online or not
    public enum PLAYSTATE
    {
        LOCAL,
        ONLINE,
        NONE
    }

    private static PLAYSTATE PlayState;

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

    //sets GameState to GAMEOVER
    public static void Gameover()
    {
        GameState = GAMESTATE.GAMEOVER;

        //this is so that the game freezes when it's gameover
        //can be removed if we don't like the look
        Time.timeScale = 0f;
    }
    
    //returns the current GameState
    public static GAMESTATE GetGameState()
    {
        return GameState;
    }

    //returns the current PlayState
    public static PLAYSTATE GetPlayState()
    {
        return PlayState;
    }

    //sets the PlayState to LOCAL
    public static void Local()
    {
        PlayState = PLAYSTATE.LOCAL;
    }

    //set GameState to MENU, Multiplay to NONE and Load Main Menu
    public static void MainMenu()
    {
        GameState = GAMESTATE.MENU;
        SceneManager.LoadScene(/*MainMenuName*/ "");
    }

    //sets the PlayState to LOCAL
    public static void NoPlayState()
    {
        PlayState = PLAYSTATE.NONE;
    }

    //sets the PlayState to ONLINE
    public static void Online()
    {
        PlayState = PLAYSTATE.ONLINE;
    }

    //sets GameState to PAUSE
    //if we choose to use this, only in local games
    public static void Pause()
    {
        GameState = GAMESTATE.PAUSE;

        //this is so that the game freezes when it's gameover
        //can be removed if we don't like the look
        Time.timeScale = 0f;
    }

    //starts the current scene again
    public static void Restart()
    {
        Start(SceneManager.GetActiveScene().name);
    }

    //set GameState to PLAYING
    //would be used to resume play after a pause
    public static void Play()
    {
        GameState = GAMESTATE.PLAYING;
        Time.timeScale = 1f;
    }

    //set GameState to PLAYING and load a/the level
    //will also be used to restart after a gameover
    //if there is only one level, we could make the string name a SerializeField instead of a parameter
    public static void Start(string Level)
    {

        GameState = GAMESTATE.PLAYING;

        //not sure if this is necessary just cuz this is for local
        SceneManager.LoadScene(Level);

        //this can be removed if Gameover() will not set timescale to 0
        Time.timeScale = 1f;
    }
}
