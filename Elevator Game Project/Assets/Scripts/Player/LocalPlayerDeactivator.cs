using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerDeactivator : MonoBehaviour
{
    void Awake()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            gameObject.SetActive(true);
        }
    }
}
