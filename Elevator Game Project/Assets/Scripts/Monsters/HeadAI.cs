using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using UnityEngine.Rendering;

public class HeadAI : Monster
{
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public GameObject[] patrolPoints;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float chaseSpeed, followSpeed, patrolSpeed;
    [SerializeField] private float followRange, chaseRange, farthestRange;
    [SerializeField] AudioSource[] audioSources;
    [SerializeField] Volume volume;
    private PhotonView view;
    private bool patrolling, following, chasing, gaveUp;
    private float chaseTime;
    private RaycastHit hit;
    Vector3 destination;
    
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        agent = GetComponent<NavMeshAgent>();
        //agent.enabled = false;
        Patrol();
        //player = null;

        if(GameStateManager.GetPlayState() == GameStateManager.PLAYSTATE.ONLINE)
        {
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject playerr in allPlayers)
            {
                PhotonView PlayerView = playerr.GetComponent<PhotonView>();
                if(PlayerView != null && PlayerView.IsMine)
                {
                    player = playerr;
                    break;
                }
            }
        }

        speed = patrolSpeed;
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
        if(chasing == true && (Time.time - chaseTime) > 5f)
        {
            Patrol();
            gaveUp = true;
            StartCoroutine(GiveUp());
        }
        //Patrol
        //Reached a destination, go to the next one
        if (Vector3.Distance(transform.position, destination) < 5)
        {
            Patrol();
        }
        if(player != null && player.activeInHierarchy)
        {
            //Follow
            if (Physics.SphereCast(transform.position, followRange, transform.forward, out hit, followRange, playerLayer) && chasing == false)
            {
                if (hit.transform.tag == "Player")
                {
                    player = hit.transform.gameObject;
                    Follow();
                }
            }
            //Keep following if they are still in distance
            if (Vector3.Distance(transform.position, player.transform.position) < followRange &&
                Mathf.Abs(transform.position.y - player.transform.position.y) < 5 && !chasing)
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
            //if (player != null || player.activeInHierarchy)
                //Keep chasing if they are still in distance
            if (Vector3.Distance(transform.position, player.transform.position) < chaseRange &&
                Mathf.Abs(transform.position.y - player.transform.position.y) < 5 && !gaveUp)
            {
                Chase();
            }
        }

        //transform.position += speed * Time.deltaTime * transform.forward;

        //transform.LookAt(destination);

        volume.weight = Mathf.Clamp01(10 / (Vector3.Distance(transform.position, player.transform.position) + 1) - 0.1f);
    }
    public void Chase()
    {
        patrolling = false;
        following = false;
        if (!chasing && !gaveUp)
        {
            agent.speed = chaseSpeed;
            destination = player.transform.position;
            agent.SetDestination(destination);
            chaseTime = Time.time;
            Debug.Log(chaseTime);
        }
        if (!gaveUp)
        {
            chasing = true;
        }
        
        //transform.LookAt(player.transform.position);
    }
    public void Follow()
    {
        patrolling = false;
        if (!following && !gaveUp)
        {
            agent.speed = followSpeed;
            destination = player.transform.position;
            agent.SetDestination(destination);
        }
        following = true;
        chasing = false;
        //transform.LookAt(player.transform.position);
        
    }

    public void Patrol()
    {
        if (patrolPoints[0] != null)
        {
            patrolling = true;
            following = false;
            chasing = false;
            agent.speed = patrolSpeed;
            destination = patrolPoints[Random.Range(0, patrolPoints.Length)].transform.position;
            //transform.LookAt(destination);
            if (agent.destination != destination)
            {
                agent.SetDestination(destination);
            }
            
        }
    }

    public override void Kill()
    {
        foreach (AudioSource a in audioSources)
        {
            a.enabled = false;
        }

        volume.enabled = false;
    }

    public void SetPatrolPoints(bool isUp)
    {
        patrolPoints = new GameObject[5];
        GameObject PointParent = GameObject.Find("Patrol Points");

        int up = 0;

        if (isUp)
        {
            up = 5;
        }

        for (int i = 0 + up; i < 5 + up; i++)
        {
            patrolPoints[i - up] = PointParent.transform.GetChild(i).gameObject;
        }
    }

    IEnumerator GiveUp()
    {
        yield return new WaitForSeconds(5);
        gaveUp = false;
        Debug.Log(gaveUp);
    }
}
