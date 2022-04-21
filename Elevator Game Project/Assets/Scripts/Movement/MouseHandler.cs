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

    private Camera cam;

    private PhotonView view;

    //sets the gamestate to playing when the player spawns
    private void Awake()
    {
        GameStateManager.Play();
    }

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        view = GetComponent<PhotonView>();
        //Get rid of the camera if it is not mine
        if (!view.IsMine)
        {
            Destroy(cam.gameObject);
        }  
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
                xRotation = Mathf.Clamp(xRotation, -90, 90);

                cam.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
            }
        }
          
    }
}
