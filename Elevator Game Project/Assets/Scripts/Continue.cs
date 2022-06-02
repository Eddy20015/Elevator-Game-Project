using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continue : MonoBehaviour
{
    [SerializeField] int level;

    // Start is called before the first frame update
    void Start()
    {
        if (GameStateManager.GetPlayState() != GameStateManager.PLAYSTATE.ONLINE)
        {
            PlayerPrefs.SetInt("Level", level);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
