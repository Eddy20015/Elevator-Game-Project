using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingStatus : MonoBehaviour
{
    [SerializeField] private GameObject PostProcess;

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.GetGameState() == GameStateManager.GAMESTATE.CINEMATIC)
        {
            PostProcess.SetActive(false);
        }
        else
        {
            PostProcess.SetActive(true);
        }
    }
}
