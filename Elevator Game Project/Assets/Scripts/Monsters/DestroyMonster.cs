using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMonster : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.GAMEOVER)
        {
            Destroy(gameObject);
        }
        else if (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.CINEMATIC && GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            Destroy(gameObject);
        }
    }
}
