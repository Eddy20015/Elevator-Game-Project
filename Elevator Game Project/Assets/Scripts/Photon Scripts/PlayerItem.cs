using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMPro.TMP_Text PlayerName;

    //ExitGames.Client.Photon.Hashtable PlayerProperties = new ExitGames.Client.Photon.Hashtable();   //photon's version of a hashtable

    public void SetPlayerInfo(Player _Player)
    {
        PlayerName.text = _Player.NickName;
    }

    public void ApplyLocalChanges()
    {
    
    }

        //there is a lot more info on the character selection portion on
        //the Blackthornprod Multiplater character selection tutorial
}
