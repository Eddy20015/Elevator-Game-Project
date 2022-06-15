using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

/// <summary>
/// Doomba AI, uses a simple behaviour tree to control the Doomba
/// </summary>
public class Doomba : Monster
{
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float range, chaseSpeed, patrolSpeed;
    //Offset for the other raycasts
    [SerializeField] private float xOffset;
    private bool patrolling;
    private PhotonView view;
    private RaycastHit hit;
    [SerializeField] GameObject head;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
        Patrol();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.GAMEOVER)
        {
            agent.isStopped = true;
            agent.SetDestination(transform.position);
        }

        //Checks if destination has been reached before going to the next destination
        if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            RunBehavior();
        }
        else if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            //Only 1 client will have it move
            if (view.IsMine)
            {
                RunBehavior();
            }
        }

        if (patrolling)
        {
            //Debug.Log(agent.destination);
        }
    }

    //Runs the behavior of the AI
    public void RunBehavior()
    {
        //Reached a destination, go to the next one
        if (agent.destination.x == agent.transform.position.x && agent.destination.z == agent.transform.position.z)
        {
            Patrol();
        }

        //(FIRST RAYCAST)Checks if a player is in line of sight, if they go out of line of sight, go back to patrolling
        if (Physics.Raycast(transform.position, transform.forward, out hit, range, playerLayer))
        {
            if (hit.transform.tag == "Player" && ChargingStationManager.chargingStationManager.NumOfCompletedStations >= 2)
            {
                Chase();
                //Animation Code would be here for beginning to chase

            }
            else if (patrolling == false)
            {
                //Patrol();
                //Animation Code would be here to go back to patrolling

            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * range, Color.white);
                //Debug.Log("Did not Hit");
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * range, Color.white);
            //Debug.Log("Did not Hit");
        }

        //(SECOND RAYCAST)Checks if a player is in line of sight, if they go out of line of sight, go back to patrolling
        if (Physics.Raycast(transform.position + transform.right * xOffset, transform.forward, out hit, range, playerLayer))
        {
            if (hit.transform.tag == "Player" && ChargingStationManager.chargingStationManager.NumOfCompletedStations >= 2)
            {
                Chase();
                //Animation Code would be here for beginning to chase

            }
            else if (patrolling == false)
            {
                //Patrol();
                //Animation Code would be here to go back to patrolling

            }
            else
            {
                Debug.DrawRay(transform.position + transform.right * xOffset, transform.forward * range, Color.white);
                //Debug.Log("Did not Hit");
            }
        }
        else
        {
            Debug.DrawRay(transform.position + transform.right * xOffset, transform.forward * range, Color.white);
            //Debug.Log("Did not Hit");
        }

        //(THIRD RAYCAST)Checks if a player is in line of sight, if they go out of line of sight, go back to patrolling
        if (Physics.Raycast(transform.position - transform.right * xOffset, transform.forward, out hit, range, playerLayer))
        {
            if (hit.transform.tag == "Player" && ChargingStationManager.chargingStationManager.NumOfCompletedStations >= 2)
            {
                Chase();
                //Animation Code would be here for beginning to chase

            }
            else if (patrolling == false)
            {
                //Patrol();
                //Animation Code would be here to go back to patrolling

            }
            else
            {
                Debug.DrawRay(transform.position - transform.right * xOffset, transform.forward * range, Color.white);
                //Debug.Log("Did not Hit");
            }
        }
        else
        {
            Debug.DrawRay(transform.position - transform.right * xOffset, transform.forward * range, Color.white);
            //Debug.Log("Did not Hit");
        }
    }

    public void Patrol()
    {
        patrolling = true;
        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Length)].transform.position);
        transform.LookAt(agent.destination);
        if (head.activeSelf)
        {
            head.SetActive(false);
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
            {
                photonView.RPC("SetHeadRPC", RpcTarget.All, false);
            }
        }
        Debug.Log(agent.destination);
    }

    public void Chase()
    {
        patrolling = false;
        //Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
        transform.LookAt(hit.transform.position);
        agent.speed = chaseSpeed;
        agent.SetDestination(hit.transform.position);
        if (!head.activeSelf)
        {
            head.SetActive(true);
            if (GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
            {
                photonView.RPC("SetHeadRPC", RpcTarget.All, true);
            }
        }

        
        
        //Debug.Log("Did Hit");
    }

    public override void Kill()
    {
        base.Kill();
    }

    [PunRPC]
    void SetHeadRPC(bool b)
    {
        if (head.activeSelf != b)
        {
            head.SetActive(b);
        }
    }
}
