using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCredits : MonoBehaviour
{
    [SerializeField] private AnimationClip credits;
    private float timePassed = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= credits.length)
        {
            GameStateManager.Start("MainMenu");
        }
    }
}
