using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMPro.TMP_InputField RoomInputField;
    [SerializeField] private TMPro.TMP_InputField PasswordInputField;
    [SerializeField] private GameObject LobbyPanel;
    [SerializeField] private GameObject RoomPanel;
    [SerializeField] private TMPro.TMP_Text RoomName;

    [SerializeField] private GameObject PasswordPanel;
    [SerializeField] private TMPro.TMP_Text PasswordPromptText;
    [SerializeField] private TMPro.TMP_InputField PasswordPanelInputField;

    private string PasswordKey = "Password";
    private string RoomToJoin;
    private string PasswordToEnter;
    private string OriginalPasswordPromptText;

    [SerializeField] private RoomItem RoomItemPrefab;
    private List<RoomItem> RoomItemsList = new List<RoomItem>();
    [SerializeField] private Transform ContentObject;

    [SerializeField] private float TimeBetweenUpdates = 1;
    private float NextUpdateTime;

    public List<PlayerItem> PlayerItemsList = new List<PlayerItem>();
    [SerializeField] private PlayerItem PlayerItemPrefab;
    [SerializeField] private Transform PlayerItemParent;

    [SerializeField] private GameObject StartButton;
    [SerializeField] private string GameScene;

    string[] regions = { "asia", "au", "cae", "eu", "in", "jp", "ru", "rue", "za", "sa", "kr", "tr", "us", "usw" };

    //start here will activate the LobbyPanel and deactivate all other panels
    //then the player will join the lobby
    private void Start()
    {
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);
        PasswordPanel.SetActive(false);
        PhotonNetwork.JoinLobby();

        OriginalPasswordPromptText = PasswordPromptText.text;
    }

    //this is for creating a new room
    //going to restrcting the room name length to be between 1 and 12 characters
    //and restricting the password name length to be at most 12 characters
    public void OnClickCreate()
    {
        if(RoomInputField.text.Length >= 1 && RoomInputField.text.Length <= 12 && PasswordInputField.text.Length <= 12)
        {
            //the options set the max players and the password based on the text in the password input field
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Password", PasswordInputField.text } };

            //believe this allows the password to be accessible from the lobby
            string[] Property = new string[] { "Password" };
            roomOptions.CustomRoomPropertiesForLobby = Property;

            PhotonNetwork.CreateRoom(RoomInputField.text, roomOptions);
            //PhotonNetwork.CreateRoom(RoomInputField.text, new RoomOptions() { MaxPlayers = 2, CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { PasswordKey, PasswordInputField.text } } });
                
        }
    }

    //called once the player has joined the room successfully
    //same scene, but now in the room and changed panels
    public override void OnJoinedRoom()
    {
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);
        RoomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    //the function that calls the UpdateRoomList after a certain period of time
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time >= NextUpdateTime)
        {
            UpdateRoomList(roomList);
            NextUpdateTime = Time.time + TimeBetweenUpdates;
        }
    }

    //will update and recreate every RoomItem based on all rooms that exist
    private void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in RoomItemsList)
        {
            Destroy(item.gameObject);
        }
        RoomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            //if the room is empty, don't show it in the list at all
            //in editor for some reason, it will show, but at least in build it wont
            if(room.PlayerCount == 0)
            {
                room.RemovedFromList = false;
            }

            //this creates the new RoomItem and gives it the password according to the room's properties
            RoomItem NewRoom = Instantiate(RoomItemPrefab, ContentObject);
            NewRoom.SetRoomName(room.Name);
            ExitGames.Client.Photon.Hashtable RoomHash = room.CustomProperties;
            NewRoom.Password = (string)RoomHash[PasswordKey];

            //room is finally added to the list
            RoomItemsList.Add(NewRoom);
        }
    }

    //calls Photon's JoinRoom based on the room's name
    public void JoinRoom(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    //called by the room item if it has a password
    public void ActivatePasswordPanel(string RoomName, string Password)
    {
        PasswordPanel.SetActive(true);
        //don't know if this is necessary
        LobbyPanel.SetActive(false);

        //temporarily saves these two for later use
        RoomToJoin = RoomName;
        PasswordToEnter = Password;
    }

    public void OnClickBackPassword()
    {
        PasswordPanel.SetActive(false);
        //only necessary if LobbyPanel is false at this point
        LobbyPanel.SetActive(true);

        //nullifies these two 
        RoomToJoin = "";
        PasswordToEnter = "";

        //resets this text
        PasswordPromptText.text = OriginalPasswordPromptText;
    }

    //using PasswordToEnter, this determines whether a player can get into a password locked room or not
    public void OnClickEnterPassword()
    {
        if (PasswordToEnter.Equals(PasswordPanelInputField.text))
        {
            PasswordPanel.SetActive(false);
            JoinRoom(RoomToJoin);
        }
        else
        {
            PasswordPromptText.text = "Wrong Password";
            PasswordPanelInputField.text = "";
        }
    }

    //leaves the room that the player is in
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //need to confirm if rooms are destrohyed when they are empty
    }

    //this send the player back to the ConnectToServer scene and disconnects them from the lobby
    public void OnClickBack()
    {
        PhotonNetwork.Disconnect();
        GameStateManager.ConnectToServerScene();
    }

    //resets the panels to the lobby when no longer in the room
    public override void OnLeftRoom()
    {
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
    }

    //recommended override, need to double check it's purpose
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    //this code updates the players that appear in the room
    private void UpdatePlayerList()
    {
        foreach (PlayerItem item in PlayerItemsList)
        {
            Destroy(item.gameObject);
        }
        PlayerItemsList.Clear();

        if(PhotonNetwork.CurrentRoom == null)
        {
            //perhaps in here delete the room entirely
            return;
        }

        foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem NewPlayerItem = Instantiate(PlayerItemPrefab, PlayerItemParent);
            NewPlayerItem.SetPlayerInfo(player.Value);
            PlayerItemsList.Add(NewPlayerItem);

            if(player.Value == PhotonNetwork.LocalPlayer)
            {
                NewPlayerItem.ApplyLocalChanges();
            }
        }
    }

    //the list is updated whenever a player enters/joins the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    //the list is updated whenever a player leaves the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    //the update here is used to activate the start button for the master client if the conditions of the room are met
    private void Update()
    {
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    public void OnClickStartButton()
    {
        GameStateManager.Start(GameScene);
        //PhotonNetwork.LoadLevel(GameScene);
    }

    public void ChangeRegion(int i)
    {
        PhotonNetwork.ConnectToRegion(regions[i]);
        Debug.Log(PhotonNetwork.CloudRegion);
    }
}
