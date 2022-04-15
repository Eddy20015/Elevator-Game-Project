using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    CharacterController characterController;

    [SerializeField]
    public float MovementSpeed = 1;

    [SerializeField]
    float SprintMultiplier = 2;

    [SerializeField]
    public float Gravity = 9.8f;

    [SerializeField]
    private float velocity = 0;

    private Camera cam;

    private GameObject interactionTarget;

    private bool canInteract;

    private PhotonView view;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        canInteract = false;
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
           (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && view.IsMine))
        {
            // player movement - forward, backward, left, right
            float horizontal = Input.GetAxis("Horizontal") * MovementSpeed;
            float vertical = Input.GetAxis("Vertical") * MovementSpeed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                characterController.Move((cam.transform.right * horizontal * SprintMultiplier + cam.transform.forward * vertical * SprintMultiplier) * Time.deltaTime);
            }
            else
            {
                characterController.Move((cam.transform.right * horizontal + cam.transform.forward * vertical) * Time.deltaTime);
            }

            // Gravity
            if (characterController.isGrounded)
            {
                velocity = 0;
            }
            else
            {
                velocity -= Gravity * Time.deltaTime;
                characterController.Move(new Vector3(0, velocity, 0));
            }

            if (Input.GetKeyDown(KeyCode.E) && canInteract)
            {
                //replace "Interaction" with whatever we name it in the Interactable script
                interactionTarget.SendMessage("Interact");
            }
        }
    }

    private void FixedUpdate()
    {
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
           (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && view.IsMine))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 2f))
            {
                if (hit.collider.tag == "Interactable")
                {
                    canInteract = true;
                    interactionTarget = hit.transform.gameObject;
                }
            }
            else
            {
                canInteract = false;
                interactionTarget = null;
            }
        }
    }
}
