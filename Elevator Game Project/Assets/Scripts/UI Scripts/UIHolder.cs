using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHolder : MonoBehaviour
{
    GameObject[] Children;

    bool Deactivated;

    // Start is called before the first frame update
    void Start()
    {
        Deactivated = false;

        Children = new GameObject[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            Children[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!Deactivated && (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.CINEMATIC || 
            GameStateManager.GetGameState() == GameStateManager.GAMESTATE.VICTORY ||
            GameStateManager.GetGameState() == GameStateManager.GAMESTATE.GAMEOVER))
        {
            Deactivated = true;
            for (int i = 0; i < transform.childCount; i++)
            {
                Children[i].SetActive(false);
            }
        }
        else if (Deactivated && GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PLAYING)
        {
            Deactivated = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                Children[i].SetActive(true);
            }
        }
    }
}
