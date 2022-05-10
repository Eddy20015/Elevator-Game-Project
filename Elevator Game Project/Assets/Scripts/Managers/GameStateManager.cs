using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameStateManager : MonoBehaviourPunCallbacks
{
    //Will be using this GameStateManager as a SceneManager that loads and changes scenes with the correct GameState

    private static GameStateManager Instance;

    //this is what kind of state the game is in itself
    public enum GAMESTATE
    {
        CINEMATIC,
        GAMEOVER,
        MENU,
        PAUSE,
        PLAYING,
        VICTORY
    }

    private static GAMESTATE GameState;

    //this is the current state of whether it is online or not
    public enum PLAYSTATE
    {
        LOCAL,
        NONE,
        ONLINE
    }

    private static PLAYSTATE PlayState;

    [SerializeField] private string MainMenuNameSetter;
    private static string MainMenuName;

    [SerializeField] private string ConnectToServerNameSetter;
    private static string ConnectToServerName;

    [SerializeField] private string LobbyNameSetter;
    private static string LobbyName;

    private GameObject PhotonMono;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameState = GAMESTATE.MENU;
            PlayState = PLAYSTATE.NONE;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }

        MainMenuName = MainMenuNameSetter;
        ConnectToServerName = ConnectToServerNameSetter;
        LobbyName = LobbyNameSetter;

        PhotonMono = GameObject.Find("PhotonMono");
    }

    //sets GameState to CINEMATICS
    public static void Cinematics()
    {
        GameState = GAMESTATE.CINEMATIC;

        Time.timeScale = 1f;
    }

    //go to the ConnectToServer scene
    public static void ConnectToServerScene()
    {
        GameState = GAMESTATE.MENU;
        PlayState = PLAYSTATE.ONLINE;
        SceneManager.LoadScene(ConnectToServerName);
    }

    //sets GameState to GAMEOVER
    public static void Gameover()
    {
        GameState = GAMESTATE.GAMEOVER;

        //this is so that the game freezes when it's gameover
        //can be removed if we don't like the look

        if(PlayState == PLAYSTATE.LOCAL)
        {
            Time.timeScale = 0f;
        }
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

    //go to the Lobby scene
    public static void Lobby()
    {
        GameState = GAMESTATE.MENU;
        PlayState = PLAYSTATE.ONLINE;
        SceneManager.LoadScene(LobbyName);
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
        SceneManager.LoadScene(MainMenuName);

        Time.timeScale = 1f;
    }

    //sets the PlayState to NONE
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
        if(PlayState == PLAYSTATE.LOCAL)
        {
            Time.timeScale = 0f;
        }
    }

    //set GameState to PLAYING
    //would be used to resume play after a pause
    public static void Play()
    {
        GameState = GAMESTATE.PLAYING;
        Time.timeScale = 1f;
    }

    //starts the current scene again
    public static void Restart()
    {
        Start(SceneManager.GetActiveScene().name);
    }
    
    //set the GameState to desired state
    //only to be used in Online for streaming purposes
    public static void SetGameState(GAMESTATE State)
    {
        if(PlayState == PLAYSTATE.ONLINE)
        {
            GameState = State;
        }
    }

    //set GameState to PLAYING and load a/the level
    //will also be used to restart after a gameover
    //if there is only one level, we could make the string name a SerializeField instead of a parameter
    public static void Start(string Level)
    {

        GameState = GAMESTATE.PLAYING;

        //not sure if this is necessary just cuz this is for local
        if(PlayState == PLAYSTATE.LOCAL)
        {
            SceneManager.LoadScene(Level);
        }
        else if (PlayState == PLAYSTATE.ONLINE)
        {
            PhotonNetwork.LoadLevel(Level);
        }

        //this can be removed if Gameover() will not set timescale to 0
        Time.timeScale = 1f;
    }

    //set the GameState to VICTORY
    public static void Victory()
    {
        GameState = GAMESTATE.VICTORY;

        //this is so that the game freezes when it's gameover
        //can be removed if we don't like the look
        Time.timeScale = 0f;
    }
}
