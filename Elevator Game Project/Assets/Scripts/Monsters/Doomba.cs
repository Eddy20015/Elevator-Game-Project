using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Doomba : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float range, chaseSpeed, patrolSpeed, thickness;
    private bool patrolling;
    private PhotonView view;
    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Patrol();
    }

    // Update is called once per frame
    void Update()
    {     
        //Checks if destination has been reached before going to the next destination
        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.LOCAL)
        {
            //Reached a destination, go to the next one
            if (agent.destination.x == agent.transform.position.x && agent.destination.z == agent.transform.position.z)
            {
                Patrol();
            }

            //Checks if a player is in line of sight, if they go out of line of sight, go back to patrolling
            if (Physics.Raycast(transform.position, transform.forward, out hit, range, playerLayer))
            {
                if (hit.transform.tag == "Player")
                {
                    Chase();
                }
                else if (patrolling == false)
                {
                    Patrol();
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.forward * range, Color.white);
                    Debug.Log("Did not Hit");
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * range, Color.white);
                Debug.Log("Did not Hit");
            }
        }
        else if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            //Only 1 client will have it move
            if (view.IsMine)
            {
                //Reached a destination, go to the next one
                if (agent.destination.x == agent.transform.position.x && agent.destination.z == agent.transform.position.z)
                {
                    Patrol();
                }

                //Checks if a player is in line of sight, if they go out of line of sight, go back to patrolling
                if (Physics.Raycast(transform.position, transform.forward, out hit, range, playerLayer))
                {
                    if (hit.transform.tag == "Player")
                    {
                        Chase();
                    }
                    else if (patrolling == false)
                    {
                        Patrol();
                    }
                }
                else
                {
                    Debug.DrawRay(transform.position, transform.forward * range, Color.white);
                    Debug.Log("Did not Hit");
                }
            }
        }
    }

    public void Patrol()
    {
        patrolling = true;
        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Length)].transform.position);
        transform.LookAt(agent.destination);
    }

    public void Chase()
    {
        patrolling = false;
        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
        transform.LookAt(hit.transform.position);
        agent.speed = chaseSpeed;
        agent.SetDestination(hit.transform.position);
        //Debug.Log("Did Hit");
    }
}
