using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EndCredits : MonoBehaviourPunCallbacks
{
    [SerializeField] private AnimationClip credits;
    private float timePassed = 0f;
    private bool Called;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= credits.length && !Called)
        {
            Called = true;
            GameStateManager.Start("MainMenu");
        }
    }
}
