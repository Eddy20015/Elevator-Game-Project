using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ButtonNoise : MonoBehaviour
{
    private GameObject[] ButtonNoiseObjects;

    public bool ShouldKillSelf;

    private AudioSource audio;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ShouldKillSelf = false;
        audio = GetComponent<AudioSource>();
        ButtonNoiseObjects = GameObject.FindGameObjectsWithTag("ButtonNoise");
        for(int i = 0; i < ButtonNoiseObjects.Length; i++)
        {
            if(ButtonNoiseObjects[i] != gameObject)
            {
                ButtonNoiseObjects[i].GetComponent<ButtonNoise>().ShouldKillSelf = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //this will make sure there is only one of these in a scene and it won't crowd up scenes
        if(ShouldKillSelf && !audio.isPlaying)
        {
            Destroy(gameObject);
        }    
    }
}
