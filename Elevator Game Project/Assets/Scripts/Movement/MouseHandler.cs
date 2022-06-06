using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseHandler : MonoBehaviour
{
    //Sensitivity
    [SerializeField]
    public float horizontalSpeed = 1f;
    [SerializeField]
    public float verticalSpeed = 1f;

    [SerializeField]
    private float xRotation = 0.0f;
    [SerializeField]
    private float yRotation = 0.0f;

    private GameObject player;

    [SerializeField] private GameObject model;

    private Camera cam;

    private PhotonView view;

    //sets the gamestate to playing when the player spawns
    private void Awake()
    {
        GameStateManager.Play();
        player = gameObject;
    }

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        view = GetComponent<PhotonView>();
        //Get rid of the camera if it is not mine
        if (view != null && !view.IsMine)
        {
            Destroy(cam.gameObject);
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    //controls mouse movement to camera movement
    void Update()
    {
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
           (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && view.IsMine))
        {
            if (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PLAYING)
            {
                float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
                float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

                yRotation += mouseX;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -45, 45);

                cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);

                //to check if player is dead
                if (model != null)
                {
                    player.transform.eulerAngles = new Vector3(0.0f, yRotation, 0.0f);
                    model.transform.rotation = player.transform.rotation;
                }
            }
        }
    }
}
