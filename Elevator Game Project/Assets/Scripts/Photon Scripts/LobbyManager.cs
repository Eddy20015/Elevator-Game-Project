using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    //Following BlackThornProd Tutorials for this section

    [SerializeField] private TMPro.TMP_InputField RoomInputField;
    [SerializeField] private GameObject LobbyPanel;
    [SerializeField] private GameObject RoomPanel;
    [SerializeField] private TMPro.TMP_Text RoomName;

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

    private void Start()
    {
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        if(RoomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(RoomInputField.text, new RoomOptions() { MaxPlayers = 2 });
        }
    }

    public override void OnJoinedRoom()
    {
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);
        RoomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(Time.time >= NextUpdateTime)
        {
            UpdateRoomList(roomList);
            NextUpdateTime = Time.time + TimeBetweenUpdates;
        }
    }

    private void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in RoomItemsList)
        {
            Destroy(item.gameObject);
        }
        RoomItemsList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomItem NewRoom = Instantiate(RoomItemPrefab, ContentObject);
            NewRoom.SetRoomName(room.Name);
            RoomItemsList.Add(NewRoom);
        }
    }

    public void JoinRoom(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //destroy empty room
    }

    public void OnClickBack()
    {
        PhotonNetwork.Disconnect();
        GameStateManager.ConnectToServerScene();
    }

    public override void OnLeftRoom()
    {
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    private void UpdatePlayerList()
    {
        foreach (PlayerItem item in PlayerItemsList)
        {
            Destroy(item.gameObject);
        }
        PlayerItemsList.Clear();

        if(PhotonNetwork.CurrentRoom == null)
        {
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
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
        PhotonNetwork.LoadLevel(GameScene);
    }
}
