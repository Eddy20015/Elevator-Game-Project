using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] float delayTime;

    [SerializeField] AudioClip[] audioClips;

    float currentDelay;

    AudioSource audioSource;

    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            currentDelay -= Time.deltaTime;

            if (playerMovement.IsSprinting())
            {
                currentDelay -= Time.deltaTime / 5;
            }
        }

        if (currentDelay <= 0)
        {
            Footstep();
            currentDelay = delayTime;
        }
    }

    void Footstep()
    {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length - 1)]);
        Debug.Log("Played Footstep");
    }
}
