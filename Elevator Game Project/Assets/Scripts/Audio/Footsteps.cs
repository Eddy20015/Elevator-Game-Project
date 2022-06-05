using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Footsteps : MonoBehaviourPunCallbacks
{
    [SerializeField] float delayTime;

    [SerializeField] AudioClip[] audioClips;

    float currentDelay;

    AudioSource audioSource;

    PlayerMovement playerMovement;

    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        view = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
          (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && view.IsMine))
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
    }

    void Footstep()
    {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length - 1)]);
        //Debug.Log("Played Footstep");
    }
}
