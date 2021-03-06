using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// The moment a player walks inside the elevator, it plays the sequence of eventsa
/// </summary>
public class IntroElevator : MonoBehaviourPunCallbacks
{
    [SerializeField] private ActivateElevator elevatorAnims;
    [SerializeField] private GameObject invisibleCollider;
    [SerializeField] private FadingScript fadingScript;
    [SerializeField] private SceneLoader Loader;

    [SerializeField] private PhotonView view;

    //PhotonBools
    private bool MasterIn;
    private bool FollowIn;
    private bool PhotonIEnumeratorCalled;

    private void Update()
    {
        //serialized field doesn't work for some reason
        if (Loader == null)
        {
            Loader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
            print("Loader is null");
        }

        //print("MasterIn is " + MasterIn);
        //print("FollowIn is " + FollowIn);

        if (!PhotonIEnumeratorCalled & MasterIn & FollowIn)
        {
            PhotonIEnumeratorCalled = true;
            StartCoroutine(BeginGame());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
            {
                StartCoroutine(BeginGame());
            }
            else
            {
                if (other.gameObject.GetPhotonView().IsMine == true)
                {
                    if (view.IsMine)
                    {
                        view.RPC("RPC_SetPhotonBool", RpcTarget.All, true, true);
                    }
                    else
                    {
                        view.RPC("RPC_SetPhotonBool", RpcTarget.All, false, true);
                    }
                }
            }
        }
    }

    //if online players leave, it has to reflect in the bools
    private void OnTriggerExit(Collider other)
    {
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            if (view.IsMine)
            {
                view.RPC("RPC_SetPhotonBool", RpcTarget.All, true, false);
            }
            else
            {
                view.RPC("RPC_SetPhotonBool", RpcTarget.All, false, false);
            }
        }
    }

    [PunRPC]
    private void RPC_SetPhotonBool(bool IsMine, bool EnterStatus)
    {
        if (IsMine)
        {
            MasterIn = EnterStatus;
        }
        else
        {
            FollowIn = EnterStatus;
        }
    }

    private IEnumerator BeginGame()
    {
        //Activate the collider
        invisibleCollider.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        //Close the elevators 
        elevatorAnims.CloseDoors();
        //GameStateManager.Cinematics();

        yield return new WaitForSeconds(3.5f);
        //Fade In
        fadingScript.FadeIn();

        yield return new WaitForSeconds(2.25f);

        //LOAD SCENE
        Loader.LoadScene();

        //Cursor.lockState = CursorLockMode.None;
    }

}
