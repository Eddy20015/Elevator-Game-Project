using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class HeadAI : Monster
{
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float chaseSpeed, followSpeed, patrolSpeed;
    [SerializeField] private float followRange, chaseRange, farthestRange;
    private PhotonView view;
    private bool patrolling, following, chasing;
    private float chaseTime;
    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
        Patrol();
        player = null;
    }

    // Update is called once per frame
    void Update()
    {
        //Don't move if game is over
        if (GameStateManager.GetGameState() == GameStateManager.GAMESTATE.GAMEOVER)
        {
            agent.isStopped = true;
            agent.SetDestination(transform.position);
        }
        //After 10 seconds of chasing go back to patrol
        if(chasing == true && Time.time - chaseTime > 10f)
        {
            Patrol();
        }
        //Patrol
        //Reached a destination, go to the next one
        if (agent.destination.x == agent.transform.position.x && agent.destination.z == agent.transform.position.z)
        {
            Patrol();
        }
        //Follow
        if (Physics.SphereCast(transform.position, followRange, transform.forward, out hit, followRange, playerLayer) && chasing == false)
        {
            if(hit.transform.tag == "Player")
            {
                player = hit.transform.gameObject;
                Follow();
            }
        }
        if(player != null)
        //Keep following if they are still in distance
        if (Vector3.Distance(transform.position, player.transform.position) < followRange && following == true)
        {
            //Debug.Log(agent.speed);
            Follow();
        }
        //Chase
        if (Physics.SphereCast(transform.position, chaseRange, transform.forward, out hit, chaseRange, playerLayer))
        {
            if (hit.transform.tag == "Player")
            {
                player = hit.transform.gameObject;
                Chase();
                chaseTime = Time.time;
                //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
            }
        }
        if (player != null)
            //Keep chasing if they are still in distance
            if (Vector3.Distance(transform.position, player.transform.position) < chaseRange && chasing == true)
            {
                Chase();
            }
    }
    public void Chase()
    {
        patrolling = false;
        following = false;
        chasing = true;
        transform.LookAt(player.transform.position);
        agent.speed = chaseSpeed;
        agent.SetDestination(player.transform.position);
    }
    public void Follow()
    {
        patrolling = false;
        following = true;
        chasing = false;
        transform.LookAt(player.transform.position);
        agent.speed = followSpeed;
        agent.SetDestination(player.transform.position);
    }

    public void Patrol()
    {
        patrolling = true;
        following = false;
        chasing = false;
        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Length)].transform.position);
        transform.LookAt(agent.destination);
    }
}
