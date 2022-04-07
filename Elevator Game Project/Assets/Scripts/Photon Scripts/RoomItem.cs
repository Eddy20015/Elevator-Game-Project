using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text RoomName;
    private LobbyManager Manager;

    private void Start()
    {
        Manager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string _RoomName)
    {
        RoomName.text = _RoomName;
    }

    public void OnClickItem()
    {
        Manager.JoinRoom(RoomName.text);
    }
}
