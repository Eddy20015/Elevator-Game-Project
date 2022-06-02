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

    [SerializeField]
    private float staminaDepletionRate = 0.1f;

    [SerializeField]
    private float staminaRechargeRate = 0.5f;

    private float stamina = 100f;

    private float maxStamina = 100f;

    private Camera cam;

    private GameObject interactionTarget;

    private bool canInteract;

    private bool canSprint = true;

    private bool isSprinting;

    [SerializeField] private Animator animator;

    [SerializeField] private GameObject model;

    private PhotonView view;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        canInteract = false;
        view = GetComponent<PhotonView>();
        model.GetComponent<Rigidbody>().freezeRotation = true;
    }

    //controls the raycast form the camera to interact with interactable objects
    private void Update()
    {
        if (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PLAYING)
        {
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
               (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && view.IsMine))
            {
                if (Input.GetKey(KeyCode.E) && canInteract)
                {
                    //replace "Interaction" with whatever we name it in the Interactable script
                    interactionTarget.SendMessage("Interact");
                    //animator.SetBool("Charging", true);
                }

                //deals with raycast for interacting with interactable objects
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

    private void FixedUpdate()
    {
        if (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.PLAYING)
        {
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL ||
               (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE && view.IsMine))
            {
                // player movement - forward, backward, left, right
                float horizontal = Input.GetAxis("Horizontal") * MovementSpeed;
                float vertical = Input.GetAxis("Vertical") * MovementSpeed;

                //whether or not the player can sprint
                if (stamina <= 0)
                {
                    stamina = 0;
                    canSprint = false;
                    //Debug.Log("Cannot sprint");
                    stamina += staminaRechargeRate;
                }
                else if (stamina >= maxStamina)
                {
                    stamina = maxStamina;
                    //Debug.Log("Ready to sprint");
                    canSprint = true;
                }
                else if (stamina > 0 && stamina < maxStamina && isSprinting == false)
                {
                    //Debug.Log("Recharging");
                    stamina += staminaRechargeRate;
                }

                //controls movement and whether the player is sprinting or just moving normally
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (canSprint && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S)))
                    {
                        if (Input.GetKey(KeyCode.W))
                        {
                            animator.SetFloat("Speed", 1f);
                        }
                        else if (Input.GetKey(KeyCode.S))
                        {
                            animator.SetFloat("Speed", -1f);
                        }

                        characterController.Move((cam.transform.right * horizontal * SprintMultiplier + cam.transform.forward * vertical * SprintMultiplier) * Time.deltaTime);
                        stamina -= staminaDepletionRate;
                        isSprinting = true;
                    }
                    else if (canSprint && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S)))
                    {
                        stamina += staminaRechargeRate;
                        animator.SetFloat("Speed", 0);
                    }
                    else if (canSprint == false)
                    {
                        animator.SetFloat("Speed", 0.5f);
                        characterController.Move((cam.transform.right * horizontal + cam.transform.forward * vertical) * Time.deltaTime);
                        isSprinting = false;
                    }
                    else
                    {
                        animator.SetFloat("Speed", 0);
                    }
                }
                else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S)) && !(Input.GetKey(KeyCode.LeftShift)))
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        animator.SetFloat("Speed", 0.5f);
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        animator.SetFloat("Speed", -0.5f);
                    }
                    characterController.Move((cam.transform.right * horizontal + cam.transform.forward * vertical) * Time.deltaTime);
                    isSprinting = false;
                }
                else
                {
                    animator.SetFloat("Speed", 0);
                    isSprinting = false;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    animator.SetBool("Charging", true);
                }

                

                //Gravity
                if (characterController.isGrounded)
                {
                    velocity = 0;
                }
                else
                {
                    velocity -= Gravity * Time.deltaTime;
                    characterController.Move(new Vector3(0, velocity, 0));
                }
            }
        }
    }

    //will be used for the bar
    public float GetStaminaProportion()
    {
        return stamina / maxStamina;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }
}
