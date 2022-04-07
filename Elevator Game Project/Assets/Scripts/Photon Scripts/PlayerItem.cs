using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMPro.TMP_Text PlayerName;
    [SerializeField] private GameObject HatButton;
    [SerializeField] private GameObject HatPanel;
    private bool HatPanelUp = false;

    //this is stuff for the hat selector stuff
    ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();   //photon's version of a hashtable
    [SerializeField] private Image HatPicture;  //probably won't be an image, but this is for the display
    //[SerializeField] private (not sure what type probably mesh)[] Hats;

    public void SetPlayerInfo(Player _Player)
    {
        PlayerName.text = _Player.NickName;
    }

    public void ApplyLocalChanges()
    {
        HatButton.SetActive(true);
    }

    public void OnClickHatButton()
    {
        if (!HatPanelUp)
        {
            HatPanel.SetActive(true);
            HatPanelUp = true;
        }
        else
        {
            HatPanel.SetActive(false);
            HatPanelUp = false;
        }

        //there is a lot more info on the character selection portion on
        //the Blackthornprod Multiplater character selection tutorial
        
    }
}
